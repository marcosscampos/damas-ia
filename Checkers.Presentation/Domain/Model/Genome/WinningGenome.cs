using Checkers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Checkers.Presentation.Domain.Model.Genome
{
    public class WinningGenome : AbstractGenome
    {
        private static readonly object Lock = new object();
        private static WinningGenome instance;
        private static string filepath = FileNameHelper.GetExecutingDirectory() + "WinningGenome.XML";

        public WinningGenome()
        {
            this.KingWorthGene = ConstantsSettings.KingWorth;
            this.KingDangerValueGene = ConstantsSettings.KingDangerValue;
            this.PawnDangerValueGene = ConstantsSettings.PawnDangerValue;
            this.PawnWorthGene = ConstantsSettings.PawnWorth;
        }

        public static WinningGenome GetWinningGenomeInstance()
        {
            if (instance == null)
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        if (File.Exists(filepath))
                        {
                            instance = XmlSerializationHelper.Deserialize<WinningGenome>(filepath);
                        }
                        else
                        {
                            //create new file and save it
                            instance = new WinningGenome();
                            instance.Serialize(filepath);
                        }
                    }
                }
            }

            return instance;
        }

        public void SetNewWinningGenome(AbstractGenome newWinner)
        {
            this.KingWorthGene = newWinner.KingWorthGene;
            this.KingDangerValueGene = newWinner.KingDangerValueGene;
            this.PawnDangerValueGene = newWinner.PawnDangerValueGene;
            this.PawnWorthGene = newWinner.PawnWorthGene;

            this.Serialize(filepath);
        }
    }
}
