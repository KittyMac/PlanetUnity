
public class cVector4 {

	public float x, y, z, w;

	public cVector4(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public static implicit operator cVector4(string value)
	{
		var elements = value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		return new cVector4(
			float.Parse(elements[0], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[1], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[2], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[3], System.Globalization.CultureInfo.InvariantCulture));
	}

	public override string ToString ()
	{
		return string.Format ("{0},{1},{2},{3}", x, y, z, w);
	}
}
