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
   
    <div class="Feedback">
    <asp:Label ID="lblFeedBack" runat="server" EnableViewState="false" />
    </div>
    <asp:Panel ID="pnlOrder" runat="server">
    
    <div style="margin-left: 10px;">
    <table>
        <tr>
            <td align="right">Choose carrier:</td>
            <td><asp:DropDownList runat="server" ID="ddlCarrier" DataTextField="Name" DataValueField="Id" /></td>
        </tr>
        <tr>
            <td align="right">Choose carrier mode:</td>
            <td><asp:DropDownList runat="server" ID="ddlCarrierMode" DataTextField="Name" DataValueField="Id" /></td>
        </tr>
    
    </table>
    </div>
    
    
    
                            
    <table class="ListOrders" width="800" id="OrderLines">
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
    <asp:Button runat="server" ID="btnSave" Text="SHIP" OnClick="btnSave_Click"
                    AccessKey="S" Style="clear: left"  />
                
    </asp:Panel>   
    <asp:Panel runat="server" ID="pnlPrint" Visible="false" BorderColor="#ccc" BorderWidth="1">
        <asp:BulletedList runat="server" ID="bullFiles" DisplayMode="HyperLink" />
        <p>
            PRINT TO:</p>
        <asp:DropDownList runat="server" ID="ddlPrinters" />
        <asp:Button runat="server" ID="btnPrint" Text="Print" OnClick="btnPrint_Click" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlReturnLabel" Visible="false">
        <div style="margin-top: 10px;">
            <asp:Button ID="btnPrintReturnLabel" runat="server" OnClick="btnPrintReturnLabel_Click" Text="Create a return label" />
        </div>
    </asp:Panel>
</asp:Content>
