using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace GestureAnalyzer
{
    [DataContract]
    class LegacyGesture : Gesture
    {
        [DataMember]
        public List<List<Point>> Points { get; set; }
    }
}
