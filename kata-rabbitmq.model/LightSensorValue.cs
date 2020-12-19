using Newtonsoft.Json;

namespace katarabbitmq.model
{
    public class LightSensorValue
    {
        public int ambient { get; set; }

        public override string ToString() => $"{base.ToString()} {JsonConvert.SerializeObject(this, Formatting.None)}";
    }
}