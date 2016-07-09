# Gridview-in-ASP.NET-MVC
Introduction
I have been asked so many times how to show gridview since there is no such controller in MVC like in ASP.NET. Therefore, I decided to write about all the common and simple ways of doing it. This article is written for one who has ASP.NET background or new in MVC.

Background
In web application, displaying data in a gridview is a common requirement. Hence, we will walkthrough possible ways of designing grid view in ASP.NET MVC.

In MVC, following are the most common ways of designing grid.

Simple grid design using foreach loop and html table: Iterating table's tr tag. This is pretty simple and a basic way of displaying records.
Webgrid: Provided by System.Web.Helpers class which render data in tabular format with supported feature like sorting, pagination and filtration.
JqGrid: It's a jquery plugin. It supports many advanced feature which gridview should have.
Kendo UI: This is from Telerik which is not free. We are not discussing about it in this article.
Prerequisites
Bootstrap
JqGrid jquery plugin
System.Web.Helpers DLL (we'll discuss more about it in Webgrid section)
Jquery UI library
Let's discuss in detail now.

Simple Grid Design Using Foreach Loop
We send List class to view page and iterate it along with <tr> tag of table. In the example shown below, we are passing List<Product> object. I will show an example product list class which is used for demo at the end of this article.

Action method: From the below code, GetSampleProducts() method is used to take sample product records which I'll show at the end of this article. Apart from that, rest of the code is only for pagination.

Hide   Copy Code
public ActionResult Index(int? pageNumber)
{
    ProductModel model = new ProductModel();
    model.PageNumber = (pageNumber == null ? 1 : Convert.ToInt32(pageNumber));
    model.PageSize = 4;

    List products = Product.GetSampleProducts();

    if (products != null)
    {
        model.Products = products.OrderBy(x => x.Id)
                  .Skip(model.PageSize * (model.PageNumber - 1))
                  .Take(model.PageSize).ToList();

        model.TotalCount = products.Count();
        var page = (model.TotalCount / model.PageSize) - 
                   (model.TotalCount % model.PageSize == 0 ? 1 : 0);
        model.PagerCount = page + 1;
    }

    return View(model);
}
Linq's Skip and Take method: In order to implement pagination, we have to fetch records in a part instead of complete records. Skip method skip record, for the first page it passes 0 and for the rest of the pages (pagesize *(pagenumber -1)). Take is similar to top from SQL query, it is used to take exact number of records. Combining both methods, see the below example.

Hide   Copy Code
Int pageSize=10; // record per page
Int pageNumber = 1; //current page number/index
var data = dbRecords.Skip(pageSize * (pageNumber-1)).Take(pageSize);
View Page: Look at the foreach code. Products list is being iterated to display all the rows.

Hide   Copy Code
<table class="table table-bordered">
<thead>
	  <tr>
	      <th>Product ID</th>
	      <th>Name</th>
	      <th>Price</th>
	      <th>Department</th>
	      <th>Action</th>
	  </tr>
</thead>
<tbody>
	  @foreach (var item in @Model.Products)
	  {
	      <tr>
	          <th scope="row">@item.Id</th>
	          <td>@item.Name</td>
	          <td>@item.Price</td>
	          <td>@item.Department</td>
	          <td><a data-value="@item.Id" 
	          href="javascript:void(0)" class="btnEdit">Edit</a></td>
	      </tr> 
	  }
</tbody>
</table>
The output is as follows:

Gridveiw in Asp.net MVC

This is a normal table, only difference is that inside tbody tag, we are using foreach to loop <tr> with list's data. Css class "table table-bordered" is used which is from bootstrap. To make it different look & feel, refer to the bootstrap doc.

WebGrid
It was introduced in MVC 4, prior to that, there was no way of doing it but to adapt any third party plugin or designing grid yourself. Webgrid displays records in tabular format. In other words, what we have seen in the above example, looping list class and making grid design, this is internally done in WebGrid helper class. The advantage of using WebGrid is, it supports inbuilt sorting, paging, filtering and ajax update. Here is the example.

Action Method: Only additional changes in this action method from the above example is that we are assigning total count in model's properties.

Hide   Copy Code
public ActionResult WebGrid()
{
    ProductModel model = new ProductModel();
    model.PageSize = 4;

    List products = Product.GetSampleProducts();

    if (products != null)
    {
        model.TotalCount = products.Count();
        model.Products = products;
    }

    return View(model);
}
View Page: We are initializing WebGrid object and binding it while passing product list class. "rowCount" properties is used to implement pagination.

Hide   Copy Code
@{
WebGrid grid = new WebGrid(null, rowsPerPage: @Model.PageSize);
grid.Bind(Model.Products, autoSortAndPage: true, rowCount: @Model.PageSize);
}
Once binding has been done, we can get html of grid using GetHtml() method. You can customize column name using grid.Column(), also see the last column from the below example how I have added edit link button.

Hide   Copy Code
@grid.GetHtml(tableStyle: "table table-bordered",
 mode: WebGridPagerModes.All,
 firstText: "<< First",
 previousText: "< Prev",
 nextText: "Next >",
 lastText: "Last >>",
    columns: grid.Columns(
    grid.Column("Id", " Id"),
    grid.Column("Name", "Name"),
    grid.Column("Price", "Price"),
    grid.Column("Department", "Department"),
    grid.Column(header: "Action",
                format: @<a data-value="@item.Id" href="java{C}<!-- no -->script:void(0)">Edit</a>
The output is as follows:

Gridveiw-using-webgrid

Note: If you are using MVC 4, then you have to add System.Web.Helpers reference manually. After adding the reference, right click on it, select properties and change 'CopyLocal' to True. Finally, rebuild the solution.

JqGrid
It is jquery plugin which is free and open source. This is completely Ajax enabled to display tabular data and to manipulate. Additionally, we can apply different Jquery UI theme, see the demo.

Action Method: There is nothing here since we will be getting product details using Ajax in json format.

Hide   Copy Code
public ActionResult jqGrid()
{
   return View();
}
This GetProducts method will be called by JqGrid Ajax function which returns the records in Json format. There are additional parameters as we have seen in the first example in order to implement pagination.

Hide   Copy Code
public ActionResult GetProducts(string sidx, string sord, int page, int rows)
{
  var products = Product.GetSampleProducts();
  int pageIndex = Convert.ToInt32(page) - 1;
  int pageSize = rows;
  int totalRecords = products.Count();
  int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            
  var data = products.OrderBy(x => x.Id)
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToList();

  var jsonData = new
  {
      total = totalPages,
      page = page,
      records = totalRecords,
      rows = data
  };

  return Json(jsonData, JsonRequestBehavior.AllowGet);
}
View: Include the required js library as per below. We are using CDN of jqgrid plugin.

Hide   Copy Code
<script src="https://cdn.jsdelivr.net/jqgrid/4.6.0/i18n/grid.locale-en.js"></script>
<script src="https://cdn.jsdelivr.net/jqgrid/4.6.0/jquery.jqGrid.min.js"></script>
This empty template will be fetched by using Ajax.

Hide   Copy Code
<table id="jqGrid"></table>
Ajax script of JqGrid:

Hide   Copy Code
var myGrid = $('#jqGrid');
myGrid.jqGrid({
	  url: '/Home/GetProducts/',
	  datatype: "json",
	  contentType: "application/json; charset-utf-8",
	  mtype: 'GET',
	  colNames: ['ProductID', 'Name', 'Price', 'Department', 'Action'],
	  colModel: [
	      { name: 'Id', key: true, width: 75 },
	      { name: 'Name', key: true, width: 200 },
	      { name: 'Price', key: true, width: 75 },
	      { name: 'Department', key: true, width: 200 },
	      { name: 'Edit', key: true, width: 100, editable: true, formatter: editButton }
	  ],
	  rowNum: 4,
	  pager: '#jqGridPager',
	  gridview: true,
	  rownumbers: true,
	  pagerpos: 'center'
});
The output is as follows:



Additional Information
1. Product.cs class

I am using sample product class which is below. Instead of using database, I am simply using list class for demo but which will not make any difference what code we have used to display gridview.

Hide   Copy Code
public static List GetSampleProducts()
{
	  return new List
	              {
	                  new Product(id:1, name: "Remote Car", price:9.99m, department:"Toys"),
	                  new Product(id:2, name: "Boll Pen", price:2.99m, department:"Stationary"),
	                  new Product(id:3, name: "Teddy Bear", price:6.99m, department:"Toys"),
	                  new Product(id:4, name: "Tennis Boll", price:6.99m, department:"Toys"),
	                  new Product(id:5, name: "Super Man", price:6.99m, department:"Toys"),
	                  new Product(id:6, name: "Bikes", price:4.99m, department:"Toys"),
	                  new Product(id:7, name: "Books", price:7.99m, department:"Stationary"),
	                  new Product(id:8, name: "Mobiles", price:5.99m, department:"Toys"),
	                  new Product(id:9, name: "Laptops", price:15.99m, department:"Toys"),
	                  new Product(id:10, name: "Note Books", price:2.99m, department:"Stationary")
	              };
}
2. Edit record using popup

In all the three examples, I'm using jquery dialog to modify records.

Here is the used code:

Hide   Copy Code
<div id="dialog" title="edit view" style="overflow: hidden;"></div>
We are using partial view for dialog box content which is load via Ajax. On edit button click from grid, we are passing product id to the GetProductById method via Ajax.

Hide   Copy Code
public ActionResult GetProductById(int id)
{
	  var products = Product.GetSampleProducts().Where(x => x.Id == id); ;
	
	  if (products != null)
	  {
	      ProductModel model = new ProductModel();
	
	      foreach (var item in products)
	      {
	          model.Name = item.Name;
	          model.Price = item.Price;
	          model.Department = item.Department;
	      }
	
	      return PartialView("_GridEditPartial", model);
	  }
	
	  return View();
}
Partial View (_GridEditPartial): This partial view will be loaded inside <div id="dialog" />. We are using Ajax.BeginForm to post edited records. Edited records is sent to UpdateProduct().

Hide   Copy Code
@model MVCGridView.Models.ProductModel
	
@using (Ajax.BeginForm("UpdateProduct", "Home", new AjaxOptions 
{ HttpMethod = "Post", UpdateTargetId = "result" }))
{
	  <div class="form-group">
	      <label for="exampleInputEmail1">Product Name</label>
Let me know your views on this topic.

You can find the same on my site.
