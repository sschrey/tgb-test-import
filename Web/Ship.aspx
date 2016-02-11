<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ship.aspx.cs" Inherits="Web.Ship" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">
        var prevcarrier;
        $(document).ready(function () {
            $('#<%= ddlCarrier.ClientID%>').focus(function () {
                prevcarrier = $(this).val();
            });
            $("#<%= ddlCarrier.ClientID%>").change(function (event, ctrl) {
                if (!confirm("Are you sure you want to change the carrier?"))
                    $(this).val(prevcarrier);
            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="page-header">
        <h2>Ship</h2>
    </div>

    <div class="Feedback">
    <asp:Label ID="lblFeedBack" runat="server" EnableViewState="false" />
    </div>
    <asp:Panel ID="pnlOrder" runat="server">

        <div class="form-horizontal">
            <div class="form-group">
                <label for="inputEmail3" class="col-sm-2 control-label">Choose carrier</label>
                <div class="col-sm-10">
                <asp:DropDownList 
                    CssClass="form-control" 
                    runat="server" 
                    ID="ddlCarrier" 
                    DataTextField="Name" 
                    DataValueField="Id"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCarrier_SelectedIndexChanged"
                    
                    
                     />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail3" class="col-sm-2 control-label">Choose carrier mode</label>
                <div class="col-sm-10">
                <asp:DropDownList CssClass="form-control" runat="server" ID="ddlCarrierMode" DataTextField="Name" DataValueField="Id" />
                </div>
            </div>
        </div>
                            
    <table class="table table-bordered"" id="OrderLines">
    <thead>
        <tr>
            <th>Line</th>
            <th>Part</th>
            <th>Qty</th>
            <th>Packs</th>
                        
        </tr>
    </thead>
                
    <asp:Repeater runat="server" ID="rptOrderLines">
        
        <ItemTemplate>
            <tr>
                <td>
                    Line
                    <asp:Label runat="server" ID="labLineNumber" Text='<%# Eval("LineNumber") %>' />
                </td>
                <td>
                    <asp:Label runat="server" ID="labPartNumber" Text='<%# Eval("PartId") %>' />
                    <br />
                    <asp:Label runat="server" ID="labPartName" Text='<%# Eval("PartName") %>' />
                    <br />
                </td>
                <td>
                    <asp:Label runat="server" ID="labQuantity" Text='<%# Eval("OrderQty") %>' />
                </td>
                <td>
               <asp:Repeater runat="server" ID="rptPacked" DataSource='<%# Eval("Packs") %>'>
                    <ItemTemplate>
                        <p>
                        
                            [<asp:Literal runat="server" ID="Literal0" Text='<%# Eval("PackedContainer.Container.Name") %>' />]
                            x
                            [<asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Qty") %>' />]</p>
                    </ItemTemplate>
                </asp:Repeater>
                </td>
              
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
    <asp:Button CssClass="btn btn-default" runat="server" ID="btnSave" Text="SHIP" OnClick="btnSave_Click"
                    AccessKey="S" Style="clear: left"  />
                
    </asp:Panel>   
    <asp:Panel runat="server" ID="pnlPrint" Visible="false" BorderColor="#ccc" BorderWidth="1">
        <div style="padding: 10px;">
            <asp:BulletedList runat="server" ID="bullFiles" DisplayMode="HyperLink" />
            <h4>Print</h4>
            <div class="form-inline">
                <div class="form-group">
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddlPrinters" />
                 </div>
                <div class="form-group">
                    <asp:Button CssClass="btn btn-default" runat="server" ID="btnPrint" Text="Print" OnClick="btnPrint_Click" 
                    OnClientClick="this.disabled = true; this.value = 'Printing...';" 
                    UseSubmitBehavior="false" />  
                </div>
           </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlReturnLabel" Visible="false">
        <div class="pull-right" style="margin-top: 10px;">
            <asp:Button CssClass="btn btn-default" ID="btnPrintReturnLabel" runat="server" OnClick="btnPrintReturnLabel_Click" Text="Create a return label" />
        </div>
        <div class="clearfix"></div>
    </asp:Panel>
</asp:Content>
