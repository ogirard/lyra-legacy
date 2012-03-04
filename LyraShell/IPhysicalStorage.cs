using System.Collections;
using System.Collections.Generic;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Interface Physical Storage
	/// </summary>
	public interface IPhysicalStorage
	{
		SortedList GetSongs();
		SortedList GetSongs(SortedList songs, string url, bool imp);

	    IList<Style> Styles { get; }

		bool Commit(SortedList internList);
		string CommitTranslations(SortedList trans);

	    void SaveStyle(Style style);
	    void DeleteStyle(Style style);
	    void SetStyleAsDefault(Style style);
	}
}