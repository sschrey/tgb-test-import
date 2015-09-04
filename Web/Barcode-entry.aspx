<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Barcode-entry.aspx.cs" Inherits="Web.Barcode_entry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>Scan SU Order</h2>
    </div>

    <div class="form-inline">
        <div class="form-group">
            <img src="Images/barcode-scanner.jpg" />
        </div>
        <div class="form-group">
            <div class="form-horizontal">
                <div class="form-group" style="width:100%; margin-bottom: 15px;">
                    <label class="col-sm-4 control-label">1. Scan order number on the pick slip:</label>
                    <div class="col-sm-8">
                    <input type="text" class="form-control" id="tbPickSlipLabel" />
                    </div>
                </div>
                <div class="form-group" style="width:100%">
                    <label class="col-sm-4 control-label">2. Scan order number on the box:</label>
                    <div class="col-sm-8">
                        <input type="text" class="form-control" id="tbBoxLabel"/> 
                    </div>
                </div>
            </div>
        </div>
    </div>
     <div style="display: inline-block">
        <div id="loading" style="display: none">
            <div><img src="images/indicator.gif" /></div>
        </div>
    </div>

<script type="text/javascript">
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
