
# CC NextGen Template


## Note
Please visit the [Source Repository on Github](https://github.com/nicksampsell/CCNextGen_Template) for the most up-to-date documentation.


## Installation
1. Download the template and link it to your project.  
2. Add the necessary code to **Program.cs** to allow the library to run.
3. Delete or rename the default **_Layout.cshtml** file (this is what the template uses and local files override the template)
4. Add the configuration items from the section below to your **appsettings.json** file
5. Add additional partials to add items to the sidebar or top menu based on the options in the sections below.
6. Delete the default CSS from /wwwroot/css/site.css (it adds extra space to the bottom of the page which affects the layout)

## Program.cs
```
using CCNextGen_Template;

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

builder.Services.ConfigureOptions(typeof(UIConfigureOptions));
builder.Services.AddRazorPages(); //if it is not already there

app.MapRazorPages();
```

## Add .AddCustomRazorConfiguration() Extension Methods
Add the .AddCustomRazorConfiguration() method to both the **.AddControllerWithViews()** and **.AddRazorPages()** in your startup code. This will allow the partials to render properly.
```
builder.Services.AddControllersWithViews().AddCustomRazorConfiguration();

builder.Services.AddRazorPages().AddCustomRazorConfiguration();
```

## Add items to _ViewImports.cshtml
Add the following items to your _ViewImports.cshtml file
```
@using CCNextGen_Template.Helpers
@using CCNextGen_Template.Models
```

## Appsettings.json
Add the following to appsettings.json
```
    "TemplateConfiguration": {
        "AppFullName": "",
        "AppShortName": "",
        "ThemeColor": "#00072d",
        "Favicon": "",
        "Logo": ""
    }
```

| Key | Meaning | Optional? |
| --- | --- | --- |
| AppFullName | Full name of application, used for title bar and alt text for logo | No |
| AppShortName | Shorter name of application, used under logo | No |
| ThemeColor | Used to alter color scheme of some browsers | Yes |
| Favicon | Icon used in bookmarks/title bar.  Defaults to Chemung County Logo if blank | Yes |
| Logo | Logo used in sidebar | Yes |


## Template Files

You can create partials with the following names to add content to various areas of the template.

| File Name | Renders | Description |
| --- | --- | --- |
| _Sidebar.cshtml | Left Sidebar | Displays under logo in left sidebar for if the **area** is not set to "admin"
| _AdminSidebar.cshtml | Left Sidebar | Displays under logo in left sidebar if the **area** is set to "admin"
| _Topbar.cshtml | Above Main Content | Displays a grey bar above the main content |

## Sections

You can add sections to individual pages, or if you want them to appear on multiple pages, add them to _ViewStart.cshtml or _ViewImports.cshtml

| SectionName | Description |
| --- | --- |
| CustomAdminLogo | Override the Admin Logo block with your own code |
| AdminSidebar | Add content to the sidebar (this will appear before the partial when the **area** is set to "admin"
| CustomLogo | Override the Logo with your own code |
| Sidebar | Add content to the sidebar (this will appear before the partial when the **area** is not set to "admin"
| HeadInject | Add content to the <head></head> section of the layout.
| Scripts | Add content before the closing body (</body>) html tag.


## Helpful Partials

### _PageHeader.cshtml
This partial will display the title of the page as well as add a button linking to the "Create" form (or if on an editor page, a link to return to the list view.  Success, error, and warning message displays are also generated from this partial.

To set the text in the partial, set the following in your view

`ViewData["Title"] = "Page Title"`

To hide the button, set the following

`ViewData["HideButton"] = true`

To add additional buttons, you can create a list of *AdditionalLink* objects:

```
ViewData["AdditionalLinks"] = new List<AdditionalLinks> {
	new AdditionalLinks {
		Page = "/Folder/MyRazorPage",
		Title = "My Page Title",
		Handler = "post" //you normally do this, but this is for demonstration purposes
	},
	new AdditionalLinks {
		Url = "http://google.com",
		Title = "Google",
		ClassName = "btn btn-secondary",
		IdName = "googleButton",
	},
	new AdditionalLinks {
		Area = "Admin",
		Controller = "Users",
		Action = "Edit",
		Data = new Dictionary<string, string> {
			{ "id", "3" }
		},
		Title = "Edit Account",
		Icon = "person"
	}
}
```

|Field | Description | Required |
| --- | --- | --- |
| Url | Absolute or Relative url to website or file | no*
| Area | Name of area (if using asp-based routing) | no*
| Controller | Controller name (if using asp-based routing) | no*
| Action | Action Name (if using asp-based routing) | no*
| Data | Dictionary of items added as route-data | no
| Title | Text used for button | yes 
| ClassName | Class used when rendering button | no
| IdName | Id used for rendering button | no
| Icon | Icon used for button.  Uses [Material Icons](https://fonts.google.com/icons) names | no
| Page | Point to a Razor Pages link using asp-page syntax. | no**
| Handler | Override Razor Page Handler.  In most cases, GET is used.

\* Either URL or asp-based routing needs to be used for the button

### _Pagination.cshtml
Generates pagination links and a drop down to change the number of items displayed on a list page.  This partial requires the package:

[https://github.com/SitholeWB/Pagination.EntityFrameworkCore.Extensions](https://github.com/SitholeWB/Pagination.EntityFrameworkCore.Extensions)

Your results need to be of the type `Pagination<TModel>` and from your controller, the following ViewData items need to be set:
|ViewData Key | Description |
| --- | --- |
| Search | Search Term (can be blank or null) |
| Page | int, default to 1 |
| PerPage | int, default to 100 |


something along the lines of the following would provide everything the pagination partial needs to function.
```
public async Task<IActionResult> Index(string? search = "", int page = 1, int perPage = 100)
{
	var users = _context.Users.AsQueryable();

	if(!String.IsNullOrEmpty(search))
	{
		users = users.where(x => x.FirstName.Contains(search) || x.LastName.Contains(search));
	}

	ViewData["Search"] = search;
	ViewData["Page"] = page;
	ViewData["PerPage"] = perPage;

	return View(new Pagination(await user.Skip((page - 1) * perPage)).Take(perPage).ToListAsync(), await user.CountAsync(), page, perPage))

}
```

## Special Sub-Layouts
These layouts extend the base layout for cases where patterns were identified in page layouts.

### _Editor.cshtml
A simple page layout for forms.  This layout loads the page header partial and the code for select2.

### _Index.cshtml
A complex layout that makes rendering list pages simple.  This layout displays the page header partial, but also displays a search form (and if the user is searching, it provides a link to "view all results.").  The Pagination is disabled at the bottom of this view.  When you load this view, all you need to worry about is creating the data table.

The necessary javascript to make the pagination work, and the links for the page header will work automatically.

To uses these pages, open the view you want to use the "sub-layout" on:

e.g., /views/users/index.cshtml
```
@model Pagination<MyNameSpace.Models.User>
@{
	ViewData["Title"] = "Manage Users";
	Layout = "SubLayouts/_Index";
}

<table class="table table-striped">
...rest of code...
</table>
```

#### Enabling Pagination
Pagination in the template requires [Pagination.EntityFrameworkCore.Extensions](https://github.com/SitholeWB/Pagination.EntityFrameworkCore.Extensions).    The template looks for the property **TotalItems** in the model before displaying the Pagination links.  The pagination links use the following query variables:

| variable | purpose | suggested default |
| -- | -- | -- |
| search | when used in conjunction with search box, keeps value when paging. | "" (empty string) or String.Empty |
| page | current page number (MVC ONLY) | 1 |
| pageSet | current page number (Razor Pages ONLY) | 1 |
| perPage | how many items per page | 100 |
| sortBy | what column to sort by | Primary Key or Timestamp Column
| desc | whether to sort in descending order | true |

In an MVC controller, an action make look like this:


```
public async Task<IActionResult> Index(string search="", int page = 1, int perPage=100, string sortBy = "ItemId", bool desc = true)
{
	//your code that is able to search for using the passed in data.
	//be sure to use Pagination.EntityFrameworkCore.Extensions 
	//and return the results to the view.	
	
	var results = ;

	ViewData["Search"] = search;
	ViewData["PerPage"] = perPage;
	ViewData["Page"] = page;
	ViewData["SortBy"] = sortBy;
	ViewData["Desc"] = desc;
	return View(await _context.FictionalTable.AsPaginationAsync(page, perPage, sortBy, desc));
}
```

In Razor Pages, it works in much the same way.  In your Handlers, pass in the search, page, perPage, sortBy, and desc parameters.  Handle them as usual, and then render them using ViewData.  (This may change in the future to better accommodate Razor Pages best practices, but for now utilize this method).    Your method may look like this:

```

[ViewData]
public string Search { get; set;} = "";
[ViewData]
public int PerPage { get; set;} = 100;
[ViewData]
public int PageSet { get; set;} = 1; //Replaces ViewData["Page"] because Page() is a reserved keyword.
[ViewData]
public string SortBy { get; set;} = "ColumnName";
[ViewData]
public bool Desc { get; set; } = true;
[ViewData]


public async Task OnGetAsync(string search="", int pageSet = 1, int perPage=100, string sortBy = "DefaultColName", bool desc = true)
{

	//Your code to populate the Razor Pages Model

	Search = serach;
	PageSet = pageSet;
	PerPage = perPage;
	SortBy = sortBy;
	Desc = desc;
	
}
```

For pagination, it is critical that you include the following items to ensure the pagination links render properly.

**ViewData["PerPage"] = perPage;**
**ViewData["Page"] = page;** //used in MVC
**ViewData["PageSet"] = pageSet;** //used in Razor Pages
**ViewData["SortBy"] = sortBy;**
**ViewData["Desc"] = desc;**

#### An Important Note For Razor Pages
Because of how Razor Pages defines and uses Page Models, you need to define an extra variable to inform the Pagination Partial of the model used for data.  To do this, either in the code-side or in the template size, define a ViewData variable called RPModel and pass in an instance of the data model.

**ViewData["RPModel"] = Model.Department;**

An example page may look like this:
```
@page
@using CCNextGen_Template.Models
@model CCSO_DeputyCertificationTracking.Pages.Departments.IndexModel

@{

    ViewData["Title"] = "Index";
    Layout = "SubLayouts/_Index";
    ViewData["RPModel"] = Model.Department;
    Layout = "SubLayouts/_Index";
}
//... rest of code
```

#### Enabling Search
The search box that is part of the SubLayouts/_Index template uses a GET form that sets the content in the search field to a parameter called search, so when pressing the search button, your url will have the following added to it ?search=SEARCH+QUERY+DATA.

To access it in your code, modify your function parameters to include a nullable string called search with an empty string as a default.  A an MVC project that uses pagination and the search function may look like this:

```
public async Task<IActionResult> Index(string search="", int page = 1, int perPage=100, string sortBy = "ItemId", bool desc = true)
{
	//your code that is able to search for using the passed in data.
	//be sure to use Pagination.EntityFrameworkCore.Extensions 
	//and return the results to the view.	

	ViewData["Search"] = search;
	ViewData["PerPage"] = perPage;
	ViewData["Page"] = page; //MVC Only
	ViewData["PageSet"] = pageSet //Razor Pages Only
	ViewData["SortBy"] = sortBy;
	ViewData["Desc"] = desc;
	return View(result);
}
```

It is important the the **ViewData["search"]** variable is set with the passed in search variable to repopulate the 


#### Hiding Search
If you do not want the search box to appear on your index page when using the SubLayouts/_Index template, add the following to your RazorView

```
ViewData["HideSearch"] = true;
```


## Layout Partial Samples

### _Sidebar.cshtml
The sidebar makes use of Bootstrap pills and Material Icons.  Below is an example of what the file may look like
```
<hr />
<ul class="nav nav-pills flex-column mb-auto flex-grow-1">
	<li>
	    <a asp-area="" asp-controller="Home" asp-action="Create"
	       class="nav-link text-white @Html.ActiveClass(controller: "Home", action: "Index", cssClass: "active")">
	        <div class="d-flex justify-content-start align-items-center g-2">
	            <i class="material-symbols-outlined me-2">home</i>
	            <span class="text-nowrap flex-grow-1">Home</span>
	        </div>
	    </a>
	</li>
</ul>
```
### Status Messages/Alerts
To use alert messages (for success messages, errors, and warnings) uses the TempData variable (or data attribute) to set the following variables:

| Key | Alert Type | Recommended Use |
| -- | -- | -- |
| success | Green Alert Box | Successful actions |
| error | Red/Pink Alert box | Errors |
| warning | Orange/Yellow Alert Box | Dangerous actions, Less-Serious Errors.  Notifications |
| info | Light Blue Alert box | Informational alerts |

Key names also work with the first letter capitalized.  (Success/success, Error/error, etc.)


#### To use the Status Messages on Redirects
In MVC, use the TempData variable
```
TempData["success"] = "Your message here";
TempData["error"] = "Your message here";
```

In Razor Pages, use the TempData variable or the [TempData] attribute helper..
```
TempData["success"] = "Your message here";

[TempData]
public string Error { get; set; }

public async Task OnGetAsync()
{
	if(someCondition)
	{
		Error = "I will display in an error message alert box after redirecting.";
	}
}
```

#### To use the Status Messages on the same Page
Set the message in ViewData.  In MVC or Razor Pages, you can use the ViewData dictionary:
```
ViewData["success"] = "Your message here";
ViewData["info"] = "Another message here";
```

In Razor pages, you can also use the **[ViewData]** attribute
```
[ViewData]
public string Error { get; set; }

public async Task OnGetAsync()
{
	if(someCondition)
	{
		Error = "I will display in an error message alert box on the same page.";
	}
}
```

### Additional Notes
To use the Yellow pill highlighting in the sidebar, add the following CSS to your stylesheet. (It is used on a per-app basis, so it is not enabled by default).

```
.nav-pills .nav-link.active
{
    background-color: #ECC435 !important;
    color: black !important;
}
```

### Overriding Templates
If you want to override any of the templates, take note of the CCNextGen_Template's file structure.  In your own project, recreate the bolded file structure and make a copy of the template you wish to override.

* **Pages**
	* _ViewImports.cshtml
	* _ViewStart.cshtml
	* **Shared**
		* _Layout.cshtml
		* **LayoutPartials**
			* _PageHeader.cshtml
			* _Pagination.cshtml
			* _ValidationScriptsPartial.cshtml
		* **SubLayouts**
			* _Editor.cshtml
			* _Index.cshtml
			* _Viewer.cshtml

For example, if you want to override the page header, you would create the following folder structure:

* Pages
	* Shared
		* LayoutPartials

then inside the **LayoutPartials** folder, you would place a copy of the _PageHeader.cshtml file from the original project into this folder (see the [GitHub Repository](https://github.com/nicksampsell/CCNextGen_Template) for the latest source code).
