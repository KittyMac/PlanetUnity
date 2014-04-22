
public class cColor {

	public float r, g, b, a;

	public cColor(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public static implicit operator cColor(string value)
	{
		var elements = value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		return new cColor(
			float.Parse(elements[0], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[1], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[2], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[3], System.Globalization.CultureInfo.InvariantCulture));
	}

	public override string ToString ()
	{
		return string.Format ("{0},{1},{2},{3}", r, g, b, a);
	}
}
