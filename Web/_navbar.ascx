<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_navbar.ascx.cs" Inherits="Web._navbar" %>

<div class="navbar navbar-inverse navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="<%= ResolveUrl("~/barcode-entry.aspx")%>">Home</a>
        </div>
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li role="presentation" class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
                        SU Orders <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a href="<%= ResolveUrl("~/Barcode-entry.aspx") %>">Barcode entry</a></li>
                        <li><a href="<%= ResolveUrl("~/List.aspx")%>">List</a></li>
                        <li><a href="<%= ResolveUrl("~/Search.aspx")%>">Search</a></li>
                    </ul>
                </li>
                <li role="presentation" class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
                        SN Orders <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                            
                        <li><a href="<%= ResolveUrl("~/SNOrders") %>">Barcode entry</a></li>
                        <li><a href="<%= ResolveUrl("~/SNOrders/Search") %>">Search</a></li>
                    </ul>
                </li>
                <li role="presentation" class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
                        TNT <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a href="<%= ResolveUrl("~/TNT/CheckPrice") %>">Check Price</a></li>
                        <li><a href="<%= ResolveUrl("~/TNT/GetLabel") %>">Label</a></li>
                          <li><a href="<%= ResolveUrl("~/TNT/Daily") %>">Daily orders</a></li>
                         <li><a href="<%= ResolveUrl("~/TNT/ManifestSummart") %>">Manifest summary</a></li>
                    </ul>
                </li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li class="navbar-text"><%= Page.User.Identity.Name%> </li>
            </ul>
        </div>
    </div>
</div>
