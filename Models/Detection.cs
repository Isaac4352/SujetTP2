using System.Collections.Generic;

public class Detection
{
    public string language { get; set; }
    public string isReliable { get; set; }
    public string confidence { get; set; }

    public Detection(string _language, string _isReliable, string _confidence) {
        
        this.language = _language;
        this.isReliable = _isReliable;
        this.confidence = _confidence;
    }
}
