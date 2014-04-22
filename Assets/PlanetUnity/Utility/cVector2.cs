
public class cVector2 {

	public float x, y;

	public cVector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public static implicit operator cVector2(string value)
	{
		var elements = value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		return new cVector2(
			float.Parse(elements[0], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[1], System.Globalization.CultureInfo.InvariantCulture));
	}

	public override string ToString ()
	{
		return string.Format ("{0},{1}", x, y);
	}
}
