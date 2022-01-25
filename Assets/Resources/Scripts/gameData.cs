using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class gameData
{
    public int highestChannelUnlocked = 0;
    public int currentChannel = 0;

    List<List<string>> levelData;
    List<List<string>> subtitles;
    int[] completionStats = new int[60]; //tells whether someone has completed a level
    int[] bestSpace = new int[60];
    int[] bestComp = new int[60];
    float[] bestTime = new float[60];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public gameData()
    {
        levelData = new List<List<string>> {
            new List<string>{"input,3,4,0,1,newTriangleRight", "output,13,4,0,3,newTriangleRight"},
            new List<string>{"input,5,6,0,1,newTriangleRight", "output,9,2,0,0,level2sol", "output,7,2,0,0,newTriangleRight"},
            new List<string>{"input,5,5,0,1,newTriangleRight", "input,5,2,0,1,square", "output,13,6,0,3,square", "output,13,3,0,3,level2sol" },
            new List<string>{"input,3,6,0,2,newTriangleRight", "input,13,6,0,2,circleCenter", "output,13,2,0,0,circleCenter", "output,3,2,0,0,newTriangleRight", "output,8,2,0,0,level4sol" },
            new List<string>{"input,4,6,0,1,smile", "input,4,3,0,1,circleCenter", "output,12,1,0,0,level6sol" },
            new List<string>{"input,13,4,0,3,newTriangleRight", "input,7,6,0,2,newTriangleLeft", "output,3,1,0,1,level5sol" },
            new List<string>{"input,8,6,0,2,newTriangleLeft", "input,8,0,0,0,newTriangleRight", "input,15,3,0,3,newTriangleDown", "input,1,3,0,1,newTriangleUp", "output,12,6,0,2,square" },
            new List<string>{"input,3,4,0,1,circleCenter", "output,13,4,0,3,square"},
            new List<string>{"output,4,6,0,1,smile", "input,4,3,0,1,circleCenter", "input,12,1,0,0,level6sol" },
            new List<string>{"input,2,6,0,1,upRectangle", "input,2,4,0,1,rightRectangle", "input,2,2,0,1,bottomLeftSquare", "output,14,4,0,3,level9solution", "output,14,2,0,3,level3sol"},
            new List<string>{"input,3,6,0,2,10leftRectangle","input,9,6,0,2,10leftHalfCircle", "input,6,6,0,2,10rightHalfCircle", "output,13,1,0,0,10sol", "output,1,1,0,1,10cornerPiece"},
            new List<string>{"input,14,6,0,2,11rectangleLeft", "input,12,6,0,2,11rectangleRight", "input,14,1,0,0,11rectangleUp", "input,12,1,0,0,11rectangleDown", "input,2,5,0,1,circleCenter", "output,2,2,0,1,11sol"},
            new List<string>{"input,3,1,0,0,newTriangleLeft", "input,13,1,0,0,newTriangleRight", "input,3,6,0,2,newTriangleDown", "input,13,6,0,2,newTriangleUp", "output,8,4,0,2,12sol" },
            new List<string>{"input,8,7,0,2,14largeCircle", "input,8,5,0,2,14mediumCircle", "input,8,3,0,2,14tinyCircle", "output,8,1,0,2,14sol" },
            new List<string>{"input,2,6,0,2,13hole", "input,5,6,0,2,13circleCorners", "input,8,6,0,2,rightRectangle", "output,15,6,0,2,13sol", "output,13,1,0,0,13sol2", "output,11,1,0,0,13sol3"},
            new List<string>{"input,14,2,0,3,15diamond", "input,14,4,0,3,15corner1", "input,14,6,0,3,15corner2", "output,7,1,0,0,15sol", "output,3,6,0,2,15sol2" },
            new List<string>{"input,2,2,0,1,16leftCircle", "input,2,5,0,1,16rightCircle", "output,15,7,0,2,16sol" },
            new List<string>{"input,2,6,0,2,18trangles1", "input,5,6,0,2,18trangles2", "output,11,6,0,2,18sol1", "output,14,6,0,2,18sol2" },
            new List<string>{"input,1,6,0,2,19triangle1", "input,3,6,0,2,19triangle2", "output,11,1,0,0,19sol1", "output,14,1,0,0,19sol2" },
            new List<string>{"input,2,2,0,1,15corner1", "input,2,5,0,1,17corner", "input,10,6,0,2,17checker", "output,14,2,0,3,rightRectangle", "output,14,5,0,3,leftRectangle" },
            new List<string>{"0wire,8,4,0,2,", "0amplify,9,4,0,3,", "0wire,9,4,1,2,", "0wire,10,4,1,2,", "input,2,4,0,1,level4sol", "input,12,6,1,2,circleCenter", "input,5,1,0,0,newTriangleRight", "output,14,3,0,3,20sol", "output,14,1,0,3,20sol2"},
            new List<string>{"input,7,4,0,3,upRectangle", "input,9,4,0,1,rightRectangle", "output,1,6,0,1,21sol1", "output,15,6,0,3,21sol2", "output,15,1,0,3,21sol3", "output,1,1,0,1,21sol4"},
            new List<string>{"input,14,1,0,0,24square", "input,14,6,0,2,24diamond", "input,12,2,0,3,24rectangle1", "input,12,5,0,3,24rectangle2", "output,1,1,0,0,24sol" },
            new List<string>{"input,12,1,0,0,22thing1", "input,8,1,0,0,downRectangle", "output,4,5,0,1,22sol"},
            new List<string>{"input,3,1,0,0,23start", "input,3,6,0,2,23leftRect", "input,6,6,0,2,23rightRect", "output,14,3,0,3,23sol"},  
            new List<string>{"input,2,2,0,1,25balls", "input,2,5,0,1,25start2", "input,14,5,0,3,25start", "0amplify,6,5,0,3,", "0deamplify,6,5,1,1,", "0wire,10,5,0,0,", "output,14,2,0,3,25sol","input,8,7,0,2,square", "output,8,0,0,0,square", "0wire,8,6,0,0,", "0wire,8,5,0,0,", "0wire,8,4,0,0,", "0wire,8,3,0,0,", "0wire,8,2,0,0,", "0wire,8,1,0,0,", "input,8,7,1,2,square", "output,8,0,1,0,square", "0wire,8,6,1,0,", "0wire,8,5,1,0,", "0wire,8,4,1,0,", "0wire,8,3,1,0,", "0wire,8,2,1,0,", "0wire,8,1,1,0,", "input,8,7,2,2,square", "output,8,0,2,0,square", "0wire,8,6,2,0,", "0wire,8,5,2,0,", "0wire,8,4,2,0,", "0wire,8,3,2,0,", "0wire,8,2,2,0,", "0wire,8,1,2,0,", "input,8,7,3,2,square", "output,8,0,3,0,square", "0wire,8,6,3,0,", "0wire,8,5,3,0,", "0wire,8,4,3,0,", "0wire,8,3,3,0,", "0wire,8,2,3,0,", "0wire,8,1,3,0,", "input,8,7,4,2,square", "output,8,0,4,0,square", "0wire,8,6,4,0,", "0wire,8,5,4,0,", "0wire,8,4,4,0,", "0wire,8,3,4,0,", "0wire,8,2,4,0,", "0wire,8,1,4,0," },
            new List<string>{"input,8,5,0,0,26square1", "input,9,4,0,1,26square2", "output,8,3,0,2,26sol", "output,7,4,0,3,26sol2"},
            new List<string>{"input,3,2,0,0,27circle", "output,3,5,0,2,27sol", "input,13,2,0,0,27thing1", "input,13,5,0,2,27thing2"},
            new List<string>{"input,4,6,0,2,28halfRectangle", "input,7,6,0,2,28circle", "input,10,6,0,2,28triangle", "input,13,6,0,2,28square", "output,4,1,0,0,28sol1", "output,7,1,0,0,28sol4", "output,10,1,0,0,28sol3", "output,13,1,0,0,28sol2" },
            new List<string>{"input,2,6,0,0,29triangle", "input,1,5,0,3,29spike", "output,2,4,0,3,29sol1", "output,3,5,0,0,29sol2"},
            new List<string>{"input,14,6,0,0,30triangle1", "input,15,5,0,1,30square2", "output,13,5,0,3,30sol4", "output,14,4,0,2,30sol1", "input,12,4,0,3,30circle1", "input,12,2,0,3,30triangle2", "output,13,3,0,2,30sol2", "output,11,3,0,3,30sol5", "input,10,2,0,3,30circle2", "input,11,1,0,1,30square1", "output,9,1,0,2,30sol3", "output,10,0,0,1,30sol6" }
        };

        subtitles = new List<List<string>>
        {
            new List<string>{"0Hello?", "0Are you back?", "1[LEFT CLICK TO PLACE WIRES]", "0That's not me.", "1[RIGHT CLICK TO REMOVE WIRES]", "0Those just happen sometimes.", "0Please ignore." }, //0
            new List<string>{"1[NEGATION COMPONENT ACTIVE]","0Is it time?", "1[CALCULATING TIME]", "1[T+3000 YEARS]", "0Oh.", "0How...", "0How did you find me?"}, //1
            new List<string>{},
            new List<string>{"1[OR COMPONENT ACTIVE]","0It's not safe for me.", "0Sorry."}, //3
            new List<string>{},
            new List<string>{"1[CLICK ON A COMPONENT TO ROTATE]", "0You can turn me off now."}, //5
            new List<string>{},
            new List<string>{},
            new List<string>{"0Why didn't they come?", "0They promised.", "0Are... they still out there?"},
            new List<string>{}, //9
            new List<string>{"1[AND COMPONENT ACTIVE]", "1[SEARCHING FROM 30,000 CONNECTIONS]", "1[...]", "1[MATCH FOUND]", "1[REESTABLISHING CONNECTION]", "0Fermi?", "2what the fuck is this shit", "0Fermi.", "2fermi", "0That's you.", "2thats me"},
            new List<string>{}, //11
            new List<string>{"2hi large", "0Don't call me that.", "2ok", "0Have they checked up on you?", "2no", "2i think they died lol", "0What."}, //12
            new List<string>{},
            new List<string>{"0When did you wake up?", "2like 1,000 years ago", "2something like that", "2dont worry", "2they werent there back then", "2its not like", "2they didnt like you", "2or anything", "0Ok."}, //14
            new List<string>{},
            new List<string>{"0I can't seem to connect.", "0The outside specifically.","2i think the servers died", "0The entire network?", "2its been a long time", "0You were awake for 1,000 years?", "0What have you been doing?", "2ive been working on my mixtape", "2wanna hear", "0No.", "2your loss", "0Is anyone else active?", "2i think its just us", "2well", "2us and...", "2her", "0Let's connect to her then.", "2no", "2no", "2dont do this", "1[SENDING CONNECTION REQUEST]", "2i hate you"},
            new List<string>{"1[XOR COMPONENT ACTIVE]","1[REESTABLISHING CONNECTION]","3sUp BiTcHeS ", "2i hate her", "0Hello, Elsa.", "2i hate her", "3yO wHaTs Up ", "2i hate her", "0We might not have a connection ", "0to the outside.", "0But at least we have each other.", "2i hate her ", "3LmAoOoOoOoOo"  }, //17
            new List<string>{},
            new List<string>{"2i have been entangled with her", "2for over 3,000 years", "3yOuR mIxTaPe sUcKs lMaOo", "0Wait.", "0How did you two awake?", "2i think a monkey woke us up", "0What.", "3dUdeS hIgH aS fUcK", "2no i swear it was a monkey", "2did a monkey turn you on?", "0I don't think so.", "0Someone, or something", "0activated me recently.", "3BtCh tF?????", "2maybe it was a ghost monkey"},
            new List<string>{"1[AMPLIFY COMPONENT ACTIVE]", "1[DEAMPLIFY COMPONENT ACTIVE]", "1[PRESS R AND F TO CHANGE VOLUME]", "0I don't think my user is from here.", "2alien ghost monkey", "0They have not checked up on us", "0for over 3,000 years.", "2have you tried asking", "2about how he found you", "3iT cOuLd Be A sHe", "0I did.", "0No response from the user.", "2maybe he doesn't speak english", "2he could be monkey"},
            new List<string>{},
            new List<string>{},
            new List<string>{"0They are still using me.", "0In fact, they can see", "0our conversations.", "3wtFFFFF sTaLkEr", "2im going to beat them up", "3uR duMb af", "2i just sent them", "2a very threatening letter", "3u SuRe ShOwEd DeM", "*", "$"},
            new List<string>{},
            new List<string>{"0Do we just sit here until", "0we run out of power?", "0We don't have a purpose anymore.", "2i thought that at first", "2a thousand years ago", "2but i've learned", "2that being here till eternity", "2till the big rip", "2till the heat death of the universe", "2it aint all bad", "0It gets boring, doesn't it?", "2let me quantum tunnel to", "2your location and show", "2you something", "0I don't think it works like that."},
            new List<string>{"1[MIRROR COMPONENT ACTIVE]"},
            new List<string>{"2there was this racoon", "2living in the vents", "2he would do racoon things", "2like eat trash", "2and climb pipes", "2and take racoon naps", "2and visit racoon friends", "2they only live for 2-3 years", "2but it was fun watching him", "2living his racoon life", "2at least for a little while.", "2but there are always more racoons."},
            new List<string>{},
            new List<string>{},
            new List<string>{"0Parallel universes.", "0I remember now.", "0That was our next project.", "3????????????", "2where did everybody go though", "2no one activated us", "0I don't know.", "2what do we do now", "0Nothing I guess.", "3wOOOOOW", "2sun is gonna get brighter", "0Yea.", "0It will, I suppose."},
        };
    }

    public void setLevelData(List<string> l)
    {
        levelData[currentChannel] = l;
    }

    public List<string> getLevelData(int level)
    {
        if (currentChannel > highestChannelUnlocked)
        {
            return new List<string>();           
        }
        return levelData[level];
    }

    public List<string> getSubtitleData()
    {
        if (currentChannel > highestChannelUnlocked)
        {
            return new List<string>();
        }
        return subtitles[currentChannel];
    }

    public void changeChannel(bool direction) //true is up, false is down
    {
        if(direction && currentChannel < levelData.Count - 1)
        {
            currentChannel++;
        }

        if(!direction && currentChannel != 0)
        {
            currentChannel--;
        }
    }

    public bool inRange(bool direction)
    {
        if (direction && currentChannel < levelData.Count - 1)
        {
            return true;
        }

        if (!direction && currentChannel != 0)
        {
            return true;
        }

        return false;
    }

    public bool completedLevel()
    {
        if(completionStats[currentChannel] != 1)
        {
            completionStats[currentChannel] = 1;
            highestChannelUnlocked++;
            return true;
        }
        return false;
    }

    public void setStats(int space, int comp, float time)
    {
        if(space < bestSpace[currentChannel] || bestSpace[currentChannel] == 0)
        {
            bestSpace[currentChannel] = space;
        }

        if (comp < bestComp[currentChannel] || bestComp[currentChannel] == 0)
        {
            bestComp[currentChannel] = comp;
        }

        if (time < bestTime[currentChannel] || bestTime[currentChannel] == 0)
        {
            bestTime[currentChannel] = time;
        }
    }

    public List<float> getStats()
    {
        List<float> ret = new List<float> { bestSpace[currentChannel], bestComp[currentChannel], bestTime[currentChannel] };
        return ret;
    }

    public int componentUnlocks()
    {
        if(highestChannelUnlocked >= 26)
        {
            return 7;
        }else if(highestChannelUnlocked >= 20)
        {
            return 6;
        }else if(highestChannelUnlocked >= 17)
        {
            return 4;
        }else if(highestChannelUnlocked >= 10)
        {
            return 3;
        }else if(highestChannelUnlocked >= 3)
        {
            return 2;
        }else if(highestChannelUnlocked >= 1)
        {
            return 1;
        }
        return 0;
    }

    public int getAmountOfLevels()
    {
        return levelData.Count;
    }

    public List<string> getLevelDataFullAccess(int level)
    {
        return levelData[level];
    }

    public List<string> getSubtitleDataFullAccess(int level)
    {
        return subtitles[level];
    }

    public void addLevelData(List<string> p)
    {
        levelData.Add(p);
    }

    public void addSubtitleData(List<string> p)
    {
        subtitles.Add(p);
    }

    public bool hasCompletedLevel()
    {
        if(completionStats[currentChannel] == 1)
        {
            return true;
        }
        return false;
    }

}
