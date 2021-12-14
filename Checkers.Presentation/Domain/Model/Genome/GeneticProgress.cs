using Checkers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Checkers.Presentation.Domain.Model.Genome
{
    [XmlRoot]
    public class GeneticProgress
    {
        public GeneticProgress() { }

        private int numberOfGames;
        private int numberOfRounds;
        private int numberOfRandomGenomeWins;

        private static readonly object Lock = new object();
        private static readonly string Filepath = FileNameHelper.GetExecutingDirectory() + @"GeneticProgress.xml";

        private static GeneticProgress instance;

        [XmlElement]
        public int NumberOfRandomGenomeWins
        {
            get => numberOfRandomGenomeWins;

            set
            {
                numberOfRandomGenomeWins = value;
                this.Serialize(Filepath);
            }
        }

        [XmlElement]
        public int NumberOfGames
        {
            get => numberOfGames;

            set
            {
                numberOfGames = value;
                this.Serialize(Filepath);
            }
        }

        public int NumberOfRounds
        {
            get => numberOfRounds;

            set
            {
                numberOfRounds = value;
                this.Serialize(Filepath);
            }
        }

        public static GeneticProgress GetGeneticProgressInstance()
        {
            if (instance == null)
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        if (File.Exists(Filepath))
                        {
                            try
                            {
                                instance = XmlSerializationHelper.Deserialize<GeneticProgress>(Filepath);
                            }
                            catch (Exception)
                            {
                                instance = new GeneticProgress();
                                instance.Serialize(Filepath);
                            }
                        }
                        else
                        {
                            instance = new GeneticProgress();
                            instance.Serialize(Filepath);
                        }
                    }
                }
            }

            return instance;
        }

        public void ResetValues()
        {
            numberOfGames = 0;
            numberOfRandomGenomeWins = 0;
            this.Serialize(Filepath);
        }

        public void Delete()
        {
            if (File.Exists(Filepath)) File.Delete(Filepath);
        }

    }
}
