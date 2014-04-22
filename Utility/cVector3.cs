
public class cVector3 {

	public float x, y, z;

	public cVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static implicit operator cVector3(string value)
	{
		var elements = value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		return new cVector3(
			float.Parse(elements[0], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[1], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[2], System.Globalization.CultureInfo.InvariantCulture));
	}

	public override string ToString ()
	{
		return string.Format ("{0},{1},{2}", x, y, z);
	}
}
