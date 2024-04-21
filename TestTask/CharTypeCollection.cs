using System.Collections.Generic;

namespace TestTask
{
	public static class CharTypeCollection
	{
		public static IReadOnlyCollection<char> Consonants = new HashSet<char>
		{
			'й',
			'ц',
			'к',
			'н',
			'г',
			'ш',
			'щ',
			'з',
			'х',
			'ъ',
			'ф',
			'в',
			'п',
			'р',
			'л',
			'д',
			'ж',
			'ч',
			'с',
			'м',
			'т',
			'ь',
			'б',
		};
		public static IReadOnlyCollection<char> Vowels = new HashSet<char>
		{
			'у',
			'е',
			'ы',
			'а',
			'о',
			'э',
			'я',
			'и',
			'ю',
			'ё',
		};
	}
}