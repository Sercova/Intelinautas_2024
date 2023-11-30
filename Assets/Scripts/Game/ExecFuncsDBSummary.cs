using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("IntelinautasSummary")]
public class ExecFuncsDBSummary
{
    [XmlArray("PlayerGamePlay")]
    [XmlArrayItem("GamePlay")]
    public List<ExecFuncsDataSummary> SummaryLine = new List<ExecFuncsDataSummary>();

    internal void AddSummaryLine(
                                    string arg_gameplay,
                                    float arg_watching,
                                    float arg_playingemptyhands,
                                    float arg_playingfullhands,
                                    float arg_paused,
                                    float arg_levelstarted,
                                    float arg_levelfinished,
                                    float arg_restarted,
                                    float arg_checkpoint,
                                    float arg_levelmenu,
                                    float arg_failed,
                                    int arg_totalWP,
                                    int arg_retry,
                                    int arg_menupause,
                                    int arg_items
                                    //,float arg_speedRun
                                    //,float arg_speedWalk
                                    //,float arg_speedIdle
                                )
    {
        ExecFuncsDataSummary summaryElement = new ExecFuncsDataSummary();
        summaryElement.PlayerName = ExecFuncsMngr.Instance.PlayerName;

        summaryElement.gameplay = arg_gameplay;
        summaryElement.watching = arg_watching;
        summaryElement.playingemptyhands = arg_playingemptyhands;
        summaryElement.playingfullhands = arg_playingfullhands;
        summaryElement.paused = arg_paused;
        summaryElement.levelstarted = arg_levelstarted;
        summaryElement.levelfinished = arg_levelfinished;
        summaryElement.restarted = arg_restarted;
        summaryElement.restarted = arg_checkpoint;
        summaryElement.levelmenu = arg_levelmenu;
        summaryElement.failed = arg_failed;
        summaryElement.totalWP = arg_totalWP;
        summaryElement.retry = arg_retry;
        summaryElement.menupause = arg_menupause;
        summaryElement.items = arg_items
        //; summaryElement.speedRun = arg_speedRun
        //; summaryElement.speedWalk = arg_speedWalk
        //; summaryElement.speedIdle = arg_speedIdle

        ; SummaryLine.Add(summaryElement);
    }

}