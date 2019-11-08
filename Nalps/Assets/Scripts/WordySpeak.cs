using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class WordySpeak : MonoBehaviour
{
    public TextAsset textFile;     // drop your file here in inspector
    public Text box;
    public Text box0;
    public Text box1;
    public Text box2;
    public Text box3;
    private int curId;
    private string name;
    private bool exit = false;
    private Dictionary<int, DNode> Nodes = new Dictionary<int, DNode>();


    void Start() {
        string text = textFile.text;  //this is the content as string
        text = text.Replace("\t", "");
        text = text.Replace("\r", "");
        string[] inputs = text.Split('\n');
        name = inputs[0] + ": ";
        for(int i = 1; i < inputs.Length && !inputs[i].Equals("[ENDFILE]");) {
            DNode dn = new DNode(inputs, ref i);
            Nodes.Add(dn.id, dn);
        }
        curId = 619;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !exit) {
            if (!disp()) {
                exit = true;
            }
        }
    }

    //Will return true until the dialog box is over
    public bool disp() {
        box.text = name + Nodes[curId].getText();
        //Only show options if we're ready for them
        if(Nodes[curId].diaReady())setDiaOptions();
        curId = Nodes[curId].nextNode();
        return curId != -1;
    }

    public bool exitDia() {
        return exit;
    }

    void setDiaOptions() {
        List<string> toAdd = Nodes[curId].options;
        toAdd.Add("");
        toAdd.Add("");
        toAdd.Add("");
        toAdd.Add("");
        for (int i = 0; i < Nodes[curId].options.Count; i++) {
            switch (i) {
                case 0:
                    box0.text = Nodes[curId].options[0];
                    break;
                case 1:
                    box1.text = Nodes[curId].options[1];
                    break;
                case 2:
                    box2.text = Nodes[curId].options[2];
                    break;
                case 3:
                    box3.text = Nodes[curId].options[3];
                    break;
                default:
                    break;
            }
        }
    }

    public void diaSelect(int id) {
        if (id > Nodes[curId].options.Count - 1) return;
        //Later using the returns here to show battle info
        curId = Nodes[curId].dest[id];
        disp();
        setDiaOptions();
    }

    public void b1Hit() {
        if (Nodes[curId].diaReady()) diaSelect(0);
    }
    public void b2Hit() {
        if (Nodes[curId].diaReady()) diaSelect(1);
    }
    public void b3Hit() {
        if (Nodes[curId].diaReady()) diaSelect(2);
    }
    public void b4Hit() {
        if (Nodes[curId].diaReady()) diaSelect(3);
    }
}

public class DNode {
    public int id;
    //Tracks place in current window
    private int cur = 0;
    public string name;
    private bool jump = false;
    public List<string> show = new List<string>();
    //Set to -1 if no condition is required
    public int condition;
    public List<string> options = new List<string>();
    public List<int> dest = new List<int>();
    public DNode(string[] s, ref int index) {
        //Clear out the blanks
        while(s[index].Length < 1) {
            index++;
        }

        string toParse = clearBrackets(s[index]);
        if (toParse.Equals("ONMEET")) {
            id = 0;
        }
        else {
            string[] names = toParse.Split(' ');
            name = names[0];
            id = Int32.Parse(names[1]);
        }
        index++;

        string toAdd = "";
        while (!s[index].Contains("[")) {
            if (s[index].StartsWith("~")) {
                show.Add(toAdd);
                toAdd = "";
            }
            else {
                toAdd += s[index] + "\n";
            }
            index++;
        }
        show.Add(toAdd);

        toParse = clearBrackets(s[index]);
        index++;

        if (toParse.Equals("END")) {
            dest.Add(-1);
            return;
        }

        if (toParse.StartsWith("OPTIONS")) {
            string[] str = toParse.Split(' ');
            for(int i = 1; i < str.Length; i++) {
                dest.Add(Int32.Parse(str[i]));
            }
            //Parse the dialog options
            while (s[index].Equals("[D]")) {
                index++;
                options.Add(s[index]);
                index++;
            }
        }
        if (toParse.StartsWith("GO")) {
            jump = true;
            string[] str = toParse.Split(' ');
            dest.Add(Int32.Parse(str[1]));
        }
    }

    public string getText() {
        int i = Math.Min(cur, show.Count-1);
        cur++;
        return show[i];
    }
    public bool endPoint() {
        return dest.Count < 1 && cur >= show.Count;
    }

    //make the jump if there's nothing to do.
    public int nextNode() {
        if(cur >= show.Count) {
            if(dest.Count == 0) {
                return -1;
            }
            if (jump) {
                return dest[0];
            }
        }
        return id;
    }

    private string clearBrackets(string s) {
        string toRet = s.Replace("[", "");
        toRet = toRet.Replace("]", "");
        return toRet;
    }

    public bool diaReady() {
        return cur >= show.Count;
    }
}