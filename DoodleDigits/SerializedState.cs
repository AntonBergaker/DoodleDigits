using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DoodleDigits {
    class SerializedState {
        public SerializedState(string content, int cursorIndex, SerializedPoint windowDimensions) {
            Content = content;
            CursorIndex = cursorIndex;
            WindowDimensions = windowDimensions;
        }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("cursor_index")]
        public int CursorIndex { get; set; }

        [JsonPropertyName("window_dimensions")]
        public SerializedPoint WindowDimensions { get; set; }
    }

    class SerializedPoint {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
