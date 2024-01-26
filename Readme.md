# CC NextGen Template

## Installation
1. Download the template and link it to your project.  
	- Use Nuget 
	- Add the library to your solution.  Then, for your main project, right click "Dependencies" in the Solution Explorer and choose "Add Dependency" and choose CCNextGen_Template.
2. Delete or rename the default **_Layout.cshtml** file (this is what the template uses and local files override the template)
3. Add the configuration items from the section below to your **appsettings.json** file
4. Add additional partials to add items to the sidebar or top menu based on the options in the sections below.

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
	new AdditionalLink {
		Url = "http://google.com",
		Title = "Google",
		ClassName = "btn btn-secondary",
		IdName = "googleButton",
	},
	new AdditionalLink {
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
