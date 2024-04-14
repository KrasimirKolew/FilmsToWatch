using FilmsToWatch.Repositories.Contracts;
using System.Text.RegularExpressions;

namespace FilmsToWatch.Repositories.Extension
{
    public static class ModelExtensions
    {
        public static string GetInformation(this IFilmModel film)
        {
            string info =  film.Title.Replace(" ","-");

            info = Regex.Replace(info, @"[^a-zA-Z0-9/-]", string.Empty);

            return info;
        }
    }
}
