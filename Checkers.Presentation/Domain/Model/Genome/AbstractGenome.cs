using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Checkers.Presentation.Domain.Model.Genome
{
    [XmlRoot]
    public class AbstractGenome
    {
        [XmlElement]
        public int KingWorthGene { get; set; }

        [XmlElement]
        public int PawnWorthGene { get; set; }

        [XmlElement]
        public int PawnDangerValueGene { get; set; }

        [XmlElement]
        public int KingDangerValueGene { get; set; }
    }
}
