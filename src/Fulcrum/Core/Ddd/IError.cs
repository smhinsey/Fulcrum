namespace Fulcrum.Core.Ddd
{
	/// <summary>
	/// Encapsulates a logical error within the domain. This differs from an exception
	/// primarily in that there is no expectation of it being handled directly at the
	/// call-site, rather, it will be received through the command pipeline mechanism.
	/// </summary>
	public interface IError
	{
		 
	}
}