using Checkers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Checkers.Presentation.Domain.Model.Genome
{
    [XmlRoot]
    public class RandomGenome : AbstractGenome
    {
        private static readonly string Filepath = FileNameHelper.GetExecutingDirectory() + "RandomGenome.xml";
        private static readonly object Lock = new object();
        private static RandomGenome instance;

        private readonly Random rng = new Random();

        public RandomGenome() => MutateGenome();

        public static RandomGenome GetRandomGenomeInstance()
        {
            if (instance == null)
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        if (File.Exists(Filepath))
                        {
                            instance = XmlSerializationHelper.Deserialize<RandomGenome>(Filepath);
                        }
                        else
                        {
                            instance = new RandomGenome();
                            instance.Serialize(Filepath);
                        }
                    }
                }
            }

            return instance;
        }

        public void MutateGenome()
        {
            WinningGenome winningGenome = WinningGenome.GetWinningGenomeInstance();

            this.KingDangerValueGene = winningGenome.KingDangerValueGene + rng.Next(-3, 3);
            this.KingWorthGene = winningGenome.KingWorthGene + rng.Next(-3, 3);
            this.PawnDangerValueGene = winningGenome.PawnDangerValueGene + rng.Next(-3, 3);
            this.PawnWorthGene = winningGenome.PawnWorthGene + rng.Next(-3, 3);
        }

        public void MutateGenomeAndSave()
        {
            MutateGenome();
            this.Serialize(Filepath);
        }
    }
}
