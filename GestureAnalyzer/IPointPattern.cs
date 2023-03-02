using System.Drawing;

namespace GestureAnalyzer
{
    public interface IPointPattern
    {
        #region Interface Properties

        Point[][] Points { get; set; }

        #endregion
    }
}
