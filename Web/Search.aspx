<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Web.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .soldto 
    {
      cursor: pointer;  
    }
    .shipto
    {
        cursor: pointer;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div style="margin-top: 10px; margin-bottom: 10px">
<asp:Label ID="lblFeedBack" runat="server" />
</div>

<asp:Panel DefaultButton="btnSearch" ID="pnlSearchCriteria" runat="server">
    <table>
        <tr>
            <td align="right">Order id:</td>
            <td>00002-SU-<asp:TextBox ID="tbOrderId" runat="server" Width="140" /></td>
        </tr>
        <tr>
            <td align="right">Tracking number:</td>
            <td><asp:TextBox ID="tbTrackingNumber" runat="server" Width="200" /> </td>
        </tr>
        <tr>
            <td valign="top" align="right">Shipped date:</td>
            <td>
                <div>
                <asp:TextBox ID="tbShippedDateFrom" runat="server" Width="200" />
                </div>
                <div>
                <asp:TextBox ID="tbShippedDateTo" runat="server" Width="200" /> 
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" align="right">Carrier:</td>
            <td><asp:DropDownList ID="ddlCarrier" DataTextField="Name" DataValueField="Id" runat="server" /> </td>
        </tr>
        <tr>
            <td>
            </td>
            <td><asp:Button ID="btnSearch" runat="server" Text="Search" 
        onclick="btnSearch_Click" /></td>
        </tr>
    </table>
</asp:Panel>

<div style="display:none;" id="dialog-modal" title="">
    <table>
        <tr>
            <td align="right">Company name:</td>
            <td><div id="companyName"></div> </td>
        </tr>
        <tr>
            <td align="right">Attention Name:</td>
            <td><div id="attentionName"></div> </td>
        </tr>
        <tr>
            <td align="right">Address Line 1:</td>
            <td><div id="addressLine1"></div></td>
        </tr>
        <tr>
            <td align="right">Address Line 2:</td>
            <td><div id="addressLine2"></div> </td>
        </tr>
        <tr>
            <td align="right">State:</td>
            <td><div id="state"></div> </td>
        </tr>
    
        <tr>
            <td align="right">City:</td>
            <td><div id="city"></div> </td>
        </tr>

        <tr>
            <td align="right">Country code:</td>
            <td><div id="countryCode"></div></td>
        </tr>
        <tr>
            <td align="right">PhoneNumber:</td>
            <td><div id="phoneNumber"></div> </td>
        </tr>
        <tr>
            <td align="right">PostalCode:</td>
            <td><div id="postalCode"></div> </td>
        </tr>
    </table>
</div>

<asp:Panel   ID="pnlListOrders" runat="server">
    <asp:Repeater   ID="repeatOrders" runat="server"
    onitemcommand="repeatOrders_ItemCommand">            
            <HeaderTemplate>
                <table class="ListOrders">
                    <thead>
                        <tr>
                            <th>Action</th>
                            <th>Order id</th>
                            <th>Company name</th>
                            <th>Attention name</th>
                            <th>AddressLine1</th>
                            <th>City</th>
                            <th>Postal code</th>
                            <th>CC</th>
                            <th>Ship to</th>
                            <th>Sold to</th>
                            <th>Status</th>
                            <th>OrderLine id</th>
                            <th>Part id</th>
                            <th>Part name</th>
                            <th>Order qty</th>
                            <th>Pack container id</th>
                            <th>Pack container name</th>
                            <th>Pack qty</th>
                            <th>Weight</th>
                            <th>Tracking #</th>
                            
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <asp:Button Visible='<%# (int)Eval("Status") == (int)ShippingService.Business.Domain.OrderStatus.Shipped %>' ID="btnShip" runat="server" CommandName="RESHIP" CommandArgument='<%# Eval("Id") %>' Text="ReShip" />
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("Id") %>
                        <asp:HiddenField ID="OrderId" runat="server" Value='<%# Eval("Id") %>' />
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.CompanyName")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.AttentionName")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.AddressLine1")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.City")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.PostalCode")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.CountryCode")%>
                    </td>
                     <td class="shipto" rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("ShipToAddress.ShipToCode")%>
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_companyName' value='<%# Eval("ShipToAddress.CompanyName") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_attentionName' value='<%# Eval("ShipToAddress.AttentionName") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_addressLine1' value='<%# Eval("ShipToAddress.AddressLine1") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_addressLine2' value='<%# Eval("ShipToAddress.AddressLine2") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_state' value='<%# Eval("ShipToAddress.State") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_city' value='<%# Eval("ShipToAddress.City") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_countryCode' value='<%# Eval("ShipToAddress.CountryCode") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_phoneNumber' value='<%# Eval("ShipToAddress.PhoneNumber") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>_ship_to_postalCode' value='<%# Eval("ShipToAddress.PostalCode") %>' />
                    </td>
                     <td class="soldto" rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("SoldToAddress.SoldToCode")%>
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_companyName' value='<%# Eval("SoldToAddress.CompanyName") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_attentionName' value='<%# Eval("SoldToAddress.AttentionName") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_addressLine1' value='<%# Eval("SoldToAddress.AddressLine1") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_addressLine2' value='<%# Eval("SoldToAddress.AddressLine2") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_state' value='<%# Eval("SoldToAddress.State") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_city' value='<%# Eval("SoldToAddress.City") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_countryCode' value='<%# Eval("SoldToAddress.CountryCode") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_phoneNumber' value='<%# Eval("SoldToAddress.PhoneNumber") %>' />
                        <input type="hidden" id='<%# Eval("Id") %>sold_to_postalCode' value='<%# Eval("SoldToAddress.PostalCode") %>' />
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("Status")%> - 
                        <%# Eval("E1Status") %>
                    </td>
                    
                    
                     <% isFirstRow = true;%>
                    <asp:Repeater ID="repeaterOrderLines" runat="server" DataSource='<%# Eval("Lines") %>'>
                        <ItemTemplate>
                            <%if (!isFirstRow){%><tr><%}%>
                                    <td>
                                    <%# Eval("Id") %></td>
                                    <td><%# Eval("PartId") %></td>
                                    <td><%# Eval("PartName") %></td>
                                    <td><%# Eval("OrderQty") %></td>
                                        <td>
                                        <asp:Repeater runat="server" ID="rptPacked" DataSource='<%# Eval("Packs") %>'>
                                            <ItemTemplate>
                                                <div style="white-space: nowrap">
                                                <%# Eval("PackedContainer.Id") %><br />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </td>
                                        <td>
                                        <asp:Repeater runat="server" ID="rptPacked2" DataSource='<%# Eval("Packs") %>'>
                                            <ItemTemplate>
                                                <div style="white-space: nowrap">
                                                <%# Eval("PackedContainer.Container.Name") %><br />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </td>
                                        <td>
                                        <asp:Repeater runat="server" ID="rptPacked3" DataSource='<%# Eval("Packs") %>'>
                                            <ItemTemplate>
                                                <div style="white-space: nowrap">
                                                <%# Eval("Qty") %>  <br />  
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </td>
                                         <td>
                                        <asp:Repeater runat="server" ID="rptPacked4" DataSource='<%# Eval("Packs") %>'>
                                            <ItemTemplate>
                                                <div style="white-space: nowrap">
                                                <%# Eval("PackedContainer.Weight") %>  <br />  
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </td>
                                        <td class="editable">
                                        <asp:Repeater runat="server" ID="rptPacked5" DataSource='<%# Eval("Packs") %>'>
                                            <ItemTemplate>
                                                <input class="orderid" type="hidden" value='<%# (Container.Parent.Parent.Parent.Parent.FindControl("OrderId") as HiddenField).Value %>' />
                                                <div style="white-space: nowrap">
                                                <label class="text_label" oldtrackingnumber='<%# Eval("PackedContainer.TrackingNumber")%>'><%# Eval("PackedContainer.TrackingNumber")%></label>
                                                <input class="text_field" type="text" oldtrackingnumber='<%# Eval("PackedContainer.TrackingNumber")%>' style="display:none; width:200px" value='<%# Eval("PackedContainer.TrackingNumber")%>' />
                                                <br />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </td>
                                    </td>
                                </tr>
                                <%isFirstRow = false; %>
                        </ItemTemplate>    
                    </asp:Repeater>
                    
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
  </asp:Panel>

  <script language="javascript" type="text/javascript">
    function containsId(el, id)
    {
        return $(el).find("[id*='" + id + "']").val();
    }

    $(function () {

        $(".shipto").click(function () {
            var self = this;

            $("#dialog-modal").find("div").each(function () {
                $(this).html(containsId(self, $(this).attr("id")));
            });

            $("#dialog-modal").dialog({
                modal: true,
                width: 500,
                title: 'Ship to'
            });
        });

        $(".soldto").click(function () {
            var self = this;

            $("#dialog-modal").find("div").each(function () {
                $(this).html(containsId(self, $(this).attr("id")));
            });

            $("#dialog-modal").dialog({
                modal: true,
                width: 500,
                title: 'Sold to'
            });
        });

        $("#<%= tbShippedDateFrom.ClientID %>").datepicker({

            dateFormat: "yy-mm-dd",
            onClose: function (selectedDate) {
                $("#<%= tbShippedDateTo.ClientID %>").datepicker("option", "minDate", selectedDate);
            }
        });
        $("#<%= tbShippedDateTo.ClientID %>").datepicker({

            dateFormat: "yy-mm-dd",
            onClose: function (selectedDate) {
                $("#<%= tbShippedDateFrom.ClientID %>").datepicker("option", "maxDate", selectedDate);
            }
        });

        $("#<%= tbShippedDateFrom.ClientID %>").watermark("From");
        $("#<%= tbShippedDateTo.ClientID %>").watermark("To");


        var onblurhandler = function (event) {
            $(this).hide();
            $(this).prev().show();
        }

        $('.text_label').css('cursor', 'pointer');
        $('.text_label').click(function () {

            tb = $(this);
            tb.hide();
            tb.next().show();
            tb.next().select();

        });
        $('.text_field').bind('blur', onblurhandler);
        $('.text_field').keypress(function (event) {
            if (event.keyCode == '13') {
                event.preventDefault();
                SendTrackingNumberUpdate(this);
            }
        });

        function SendTrackingNumberUpdate(tb) {
            var myoldtrackingnumber = $(tb).attr("oldtrackingnumber");
            var mynewtrackingnumber = $(tb).val();

            $.ajax({
                type: "POST",
                url: "Search.aspx/SendTrackingNumberUpdate",
                data: "{'oldTrackingNumber': '" + myoldtrackingnumber + "','newTrackingNumber': '" + $(tb).val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (request, error) {
                    alert(request.responseText);
                },
                success: function (ret) {
                    $("label[oldtrackingnumber='" + myoldtrackingnumber + "']").html(mynewtrackingnumber);
                    alert("Trackingnumber updated");
                }
            });
        }

    });
</script>
</asp:Content>


