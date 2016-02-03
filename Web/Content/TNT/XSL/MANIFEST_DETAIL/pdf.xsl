<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:fo="http://www.w3.org/1999/XSL/Format"
    xmlns:url="http://whatever/java/java.net.URLEncoder"
    xmlns="http://www.w3.org/2000/svg"
    
    >
    <xsl:output method="xml" indent="yes" /> 
    <xsl:param name="barcode_url"  />
    
    <xsl:template match="/manifestdetail"> 
        <fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format"> 
            <fo:layout-master-set> 
                <fo:simple-page-master master-name="expressLabel"
                    page-height="21.0cm" page-width="27.9cm" 
                    margin="0.5cm">
                    <fo:region-body margin-top="2.9cm" />
                    <fo:region-before region-name="content-right-before" extent="2.8cm"/>
                </fo:simple-page-master> 
            </fo:layout-master-set> 
            <fo:page-sequence id="my-sequence-id" master-reference="expressLabel">
                <fo:static-content flow-name="content-right-before">
                    <fo:block-container font-size="10px" border-bottom="1px solid black">
                        <fo:block-container text-align="center" position="absolute"  left="0cm"
                            top="0px">
                            <fo:block text-decoration="underline">
                                COLLECTION MANIFEST (DETAIL) -
                                <fo:retrieve-marker retrieve-class-name="shippingoption"/>
                                (SENDER PAYS) - 72
                            </fo:block>
                            <fo:block font-weight="bold">
                                <xsl:text>TNT Express Benelux</xsl:text>
                            </fo:block>
                            <fo:block text-align="left">
                                <fo:table>
                                    
                                    <fo:table-column column-width="proportional-column-width(1)"/>
                                    <fo:table-column column-width="3cm"/>
                                    <fo:table-column column-width="3cm"/>
                                    <fo:table-column column-width="proportional-column-width(1)"/>
                                    <fo:table-body>
                                        <fo:table-row text-align="center">
                                            <fo:table-cell margin-right="5px" text-align="right" column-number="2" font-weight="bold">
                                                 <fo:block>
                                                     Shipment Date: 
                                                     
                                                 </fo:block>
                                             </fo:table-cell>
                                            <fo:table-cell text-align="left" column-number="3" >
                                                <fo:block>
                                                    <xsl:value-of select="printdate"/>
                                                </fo:block>
                                            </fo:table-cell>
                                        </fo:table-row>
                                        <fo:table-row>
                                            <fo:table-cell margin-right="5px" text-align="right" column-number="2" font-weight="bold">
                                                 <fo:block>
                                                     Pickup Id: 
                                                 </fo:block>
                                             </fo:table-cell>
                                            <fo:table-cell text-align="left" column-number="3">
                                                <fo:block>
                                                    TNT Express
                                                </fo:block>
                                            </fo:table-cell>
                                        </fo:table-row>
                                    </fo:table-body>
                                </fo:table>
                            </fo:block>
                        </fo:block-container>
                        
                        <fo:block-container 
                            position="absolute" 
                            left="0cm"
                            text-align="right"
                            top="0px">
                            <fo:block text-align="right" font-weight="bold">
                                Page <fo:page-number/> of <fo:page-number-citation-last  ref-id="my-sequence-id"/>
                            </fo:block>
                            
                            <fo:block>
                                <fo:table>
                                    
                                    <fo:table-column column-width="proportional-column-width(1)"/>
                                    <fo:table-column column-width="3cm"/>
                                    <fo:table-column column-width="2cm"/>
                                    <fo:table-body>
                                        <fo:table-row text-align="center">
                                            <fo:table-cell margin-right="5px" text-align="right" column-number="2" font-weight="bold">
                                                <fo:block>
                                                    Printed on:
                                                    
                                                </fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell text-align="right" column-number="3" >
                                                <fo:block>
                                                    <xsl:value-of select="printdate"/>
                                                </fo:block>
                                            </fo:table-cell>
                                        </fo:table-row>
                                        <fo:table-row>
                                            <fo:table-cell margin-right="5px" text-align="right" column-number="2" font-weight="bold">
                                                <fo:block>
                                                    Printed at:
                                                </fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell text-align="right" column-number="3">
                                                <fo:block>
                                                    <xsl:value-of select="printtime"/>
                                                </fo:block>
                                            </fo:table-cell>
                                        </fo:table-row>
                                    </fo:table-body>
                                </fo:table>
                            </fo:block>
                        </fo:block-container>
                        <fo:block-container>
                            <fo:block>
                                <fo:external-graphic src="url('logo.jpg')"
                                    content-height="27px"/>
                            </fo:block>
                            <fo:table>
                                <fo:table-column column-width="3cm"/>
                                <fo:table-column column-width="10cm"/>
                                <fo:table-body>
                                    <fo:table-row>
                                        <fo:table-cell padding-right="5px" text-align="right">
                                            <fo:block font-weight="bold">Sender Account:</fo:block>   
                                        </fo:table-cell>
                                        <fo:table-cell>
                                            <fo:block><xsl:value-of select="account/accountNumber"/></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row>
                                        <fo:table-cell padding-right="5px"   text-align="right">
                                            <fo:block font-weight="bold">Sender Name:</fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell>
                                            <fo:block><xsl:value-of select="sender/name"/></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row>
                                        <fo:table-cell  padding-right="5px"  text-align="right" >
                                            <fo:block font-weight="bold">Sender Address:</fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell>
                                            <fo:block>
                                                <xsl:value-of select="sender/addressLine1"/> 
                                            </fo:block>
                                            <fo:block>
                                                <xsl:value-of select="sender/town"/>, 
                                                <xsl:value-of select="sender/postcode"/>, 
                                                <xsl:value-of select="sender/country"/>
                                            </fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                </fo:table-body>
                            </fo:table>
                            
                        </fo:block-container>
                        
                    </fo:block-container>
                </fo:static-content>
                
                <fo:flow flow-name="xsl-region-body">
                    <xsl:for-each select="shippingoption">
                        <xsl:if test="position()>1">
                            <fo:block page-break-before="always"/>
                        </xsl:if>
                        <fo:block>
                          <fo:marker marker-class-name="shippingoption">
                              <xsl:value-of select="@title"/>
                          </fo:marker>
                        </fo:block>
                        <fo:block>
                            <xsl:for-each select="consignment">
                                <fo:block page-break-inside="avoid">
                                 <fo:table  font-size="8px">
                                   <fo:table-column column-number="1" column-width="11%" />
                                   <fo:table-column column-number="2" column-width="9%" /> 
                                   <fo:table-column column-number="3" column-width="7%" /> 
                                   <fo:table-column column-number="4" column-width="7%" />
                                   <fo:table-column column-number="5" column-width="10%" />
                                   <fo:table-column column-number="6" column-width="16%" />
                                   <fo:table-column column-number="7" column-width="10%" />
                                   <fo:table-column column-number="8" column-width="10%" />
                                   <fo:table-column column-number="9" column-width="15%" />
                                   <fo:table-column column-number="10" column-width="5%" />
                                   
                                   <fo:table-body>
                                        <fo:table-row>
                                            <fo:table-cell number-columns-spanned="5">
                                                <fo:block margin-top="5px">
                                                    <fo:external-graphic content-width="300px" content-height="25px">
                                                        <xsl:attribute name="src">
                                                            url('<xsl:value-of select="concat($barcode_url,barcode)" />')
                                                        </xsl:attribute> 
                                                    </fo:external-graphic>  
                                                </fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell  number-columns-spanned="2" text-decoration="underline">
                                                <fo:block margin-top="5px">Special Instructions</fo:block>
                                                
                                            </fo:table-cell>
                                            <fo:table-cell  number-columns-spanned="3" text-align="right">
                                                <fo:block margin-top="5px">
                                                <xsl:value-of select="dangerousgoods"/>
                                                DANGEROUS GOODS
                                                </fo:block>
                                            </fo:table-cell>
                                        </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="center" number-columns-spanned="5">
                                               <fo:block>
                                                   <xsl:value-of select="number"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="2">
                                               <fo:block>SENDER PAYS</fo:block>
                                           </fo:table-cell>
                                         
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>
                                                   Sender Contact:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>Distribution Center</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5px" font-weight="bold">
                                               <fo:block>Tel:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block></fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5px" font-weight="bold">
                                               <fo:block>Sender Ref:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block><xsl:value-of select="shipperref"/> </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell font-weight="bold">
                                               <fo:block>Receiver VAT Nr:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block></fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>Receiver Acc Number:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>NK</fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>
                                                   Receiver Name:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="6">
                                               <fo:block>
                                                   <xsl:value-of select="receiver/name"/>, 
                                                   <xsl:value-of select="receiver/street1"/>
                                                   <xsl:if test="receiver/street2 != ''">, </xsl:if> 
                                                   <xsl:value-of select="receiver/street2"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" margin-right="5px" font-weight="bold">
                                               <fo:block>
                                                   Serv:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="2">
                                               <fo:block>
                                                   <xsl:value-of select="shippingservicecode"/>
                                               </fo:block>   
                                           </fo:table-cell>  
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>
                                                   Receiver Adress:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="8">
                                               <fo:block>
                                                   <xsl:value-of select="receiver/city"/>,
                                                   <xsl:value-of select="receiver/postcode"/>,
                                                   <xsl:value-of select="receiver/country"/>
                                               </fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5"  font-weight="bold">
                                               <fo:block>
                                                   Receiver tel:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="3">
                                               <fo:block>
                                                   <xsl:value-of select="receiver/phone"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5px" font-weight="bold">
                                               <fo:block>Receiver Contact:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="2">
                                               <fo:block>
                                                   <xsl:value-of select="receiver/contact"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" margin-right="5px" font-weight="bold">
                                               <fo:block>
                                                   Opts:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="2">
                                               <fo:block>
                                                   <xsl:value-of select="shippingoptioncode"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>Collection:</fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>Collection Address:</fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>Delivery:</fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>Delivery Address:</fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell text-align="right" padding-right="5" font-weight="bold">
                                               <fo:block>No pieces:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>
                                                   <xsl:value-of select="pieces"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5px" font-weight="bold">
                                               <fo:block>Weight:</fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>
                                                   <xsl:value-of select="weight"/>
                                                   kgs
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5px" font-weight="bold">
                                               <fo:block>
                                                   Insurance value:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>
                                                   <xsl:value-of select="insurancevalue"/>
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell text-align="right" padding-right="5px" font-weight="bold">
                                               <fo:block>
                                                   Invoice value:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>
                                                   <xsl:value-of select="invoicevalue"/>
                                               </fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell number-columns-spanned="4" font-weight="bold">
                                               <fo:block>
                                                   Description (Including packing and marks)
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="4" font-weight="bold">
                                               <fo:block>
                                                   Dimensions (L x W x H)
                                               </fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell number-columns-spanned="4">
                                               <fo:block><xsl:value-of select="description"/> </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell number-columns-spanned="4">
                                               <fo:block><xsl:value-of select="dimensions"/> </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell font-weight="bold">
                                               <fo:block>
                                                   Total Consignment Volume:
                                               </fo:block>
                                           </fo:table-cell>
                                           <fo:table-cell>
                                               <fo:block>
                                                   <xsl:value-of select="volume"/> m³    
                                               </fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                       <fo:table-row>
                                           <fo:table-cell number-columns-spanned="10" border-bottom="1px solid black">
                                               <fo:block></fo:block>
                                           </fo:table-cell>
                                       </fo:table-row>
                                   </fo:table-body>
                               </fo:table>
                             </fo:block>
                            </xsl:for-each>
                        </fo:block>
                    </xsl:for-each>
                </fo:flow>
            </fo:page-sequence>
        </fo:root>
    </xsl:template> 
</xsl:stylesheet>