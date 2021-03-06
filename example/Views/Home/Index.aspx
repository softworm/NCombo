﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: ViewData["Message"] %></h2>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
    </p>
    <div id="tabs"></div>

    <div>
        <div style="float:left;">
            Alloy Calendar:
            <input type="text" id="alloyTxt" />
            <div id="alloyCalendar"></div>
        </div>
        <div style="float:right">
            <div>YUI 2 calendar:</div>
            <div id="yui2Calendar"></div>
        </div>
    </div>
</asp:Content>
