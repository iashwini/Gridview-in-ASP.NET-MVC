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

You can find the same on my site.
