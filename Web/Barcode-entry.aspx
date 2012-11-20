<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Barcode-entry.aspx.cs" Inherits="Web.Barcode_entry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




<div style="width: 1000px">
    <img style="vertical-align:middle" src="Images/barcode-scanner.jpg"  />
    <div style="display: inline-block; vertical-align: middle;">
        <div>1. Scan the order number on the pick slip:</div>
        <input type="text" id="tbPickSlipLabel" />
        <br /><br />
        <div>2. Scan the order number on the box:</div>
        <input type="text" id="tbBoxLabel" />
        <div style="display: inline-block">
            <div id="loading" style="display: none">
              <div><img src="images/indicator.gif" /></div>
            </div>
        </div>

    </div>
</div>

<script language="javascript" type="text/javascript">
    $(function () {
        $("#tbPickSlipLabel").focus();
        
    });

    $("#tbPickSlipLabel").keypress(function (e) {
        if (e.keyCode == 13) {
            $("#tbBoxLabel").focus();
        }
    });


    $("#tbBoxLabel").keypress(function (e) {
        if (e.keyCode == 13) {
            $("#loading").show();
            $.ajax({
                type: "POST",
                url: "Barcode-entry.aspx/BarCodePosted",
                data: "{'pickSlipScan': '" + $("#tbPickSlipLabel").val() + "','boxScan': '" + $("#tbBoxLabel").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (request, error) {
                    alert(request.responseText);
                },
                success: function (ret) {
                    $("#loading").hide();
                    if (ret.d.RedirectURL != null)
                        location.href = ret.d.RedirectURL;
                    else
                        alert(ret.d.Message);
                }
            });

        }
    });
</script>

</asp:Content>
