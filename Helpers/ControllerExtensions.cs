using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCNextGen_Template.Helpers
{
    /// <summary>
    /// Extension methods for the <see cref="Controller"/> class.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Sets the pagination view data for the controller.
        /// </summary>
        /// <typeparam name="T">The type of the extra data dictionary values.</typeparam>
        /// <param name="controller">The controller instance.</param>
        /// <param name="search">The search string.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="perPage">The number of items per page.</param>
        /// <param name="sortBy">The field to sort by.</param>
        /// <param name="desc">A flag indicating whether to sort in descending order.</param>
        /// <param name="extraData">Optional extra data dictionary.</param>
        public static void SetPaginationViewData(
            this Controller controller,
            string? search,
            int? page,
            int? perPage,
            string? sortBy,
            bool? desc,
            Dictionary<string, object>? extraData = null)
        {

            controller.ViewData["Search"] = search;
            controller.ViewData["Page"] = page ?? 1;
            controller.ViewData["PerPage"] = perPage ?? 100;
            if (!string.IsNullOrEmpty(sortBy))
            {
                controller.ViewData["SortBy"] = sortBy;
            }
            controller.ViewData["Desc"] = desc.GetValueOrDefault(false);

            if (extraData != null)
            {
                foreach (var kvp in extraData)
                {
                    controller.ViewData[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
