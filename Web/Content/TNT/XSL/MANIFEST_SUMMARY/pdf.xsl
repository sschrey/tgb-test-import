<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:fo="http://www.w3.org/1999/XSL/Format"
    xmlns:url="http://whatever/java/java.net.URLEncoder">
    <xsl:output method="xml" indent="yes" /> 
    <xsl:template match="/manifestsummary"> 
        <fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format"> 
            <fo:layout-master-set> 
                <fo:simple-page-master master-name="expressLabel"
                    page-height="27.9cm" page-width="21.0cm"
                    margin="0.5cm">
                    <fo:region-body margin-top="4.1cm" margin-bottom="4.1cm" />
                    <fo:region-before region-name="content-right-before" extent="4cm"/>
                    <fo:region-after region-name="content-right-after" extent="4cm"/>
                </fo:simple-page-master> 
            </fo:layout-master-set> 
            <fo:page-sequence id="my-sequence-id" master-reference="expressLabel">
                <fo:static-content flow-name="content-right-before">
                    <fo:block-container font-size="10px">
                        <fo:block-container 
                            text-align="center" 
                            >
                            <fo:block text-decoration="underline">
                                <xsl:text>COLLECTION MANIFEST (SUMMARY) - 72</xsl:text>
                            </fo:block>
                            <fo:block>
                                <xsl:text>TNT Express Benelux</xsl:text>
                            </fo:block>
                        </fo:block-container>
                        <fo:block-container 
                            position="absolute" 
                            left="16cm" 
                            top="0px"
                           >
                            <fo:block text-align="right">
                                <fo:external-graphic src="url('logo.jpg')"
                                    content-height="27px"/>
                            </fo:block>
                            <fo:block text-align="right">
                                Page: <fo:page-number/> of <fo:page-number-citation-last  ref-id="my-sequence-id"/>
                            </fo:block>
                            <fo:block text-align="right">
                                Date: <xsl:value-of select="printdate"/>
                            </fo:block>
                        </fo:block-container>
                        <fo:block-container >
                            <fo:table>
                                <fo:table-column column-width="3cm"/>
                                <fo:table-column column-width="10cm"/>
                                <fo:table-body>
                                    <fo:table-row>
                                        <fo:table-cell>
                                            <fo:block>Sender Account:</fo:block>   
                                        </fo:table-cell>
                                        <fo:table-cell>
                                            <fo:block><xsl:value-of select="account/accountNumber"/></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row>
                                        <fo:table-cell>
                                            <fo:block>Sender Name:</fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell>
                                            <fo:block><xsl:value-of select="sender/name"/></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row>
                                        <fo:table-cell >
                                            <fo:block>Sender Address:</fo:block>
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
                <fo:static-content flow-name="content-right-after">
                    <fo:block-container font-size="9px">
                        <fo:block>The consignment(s) has been loaded by TNT or a designated agent of TNT and a check of the number and condition of the consignment(s) has been undertaken by TNT</fo:block>    
                        <fo:block margin-top="30px" text-align="right">
                            Print Date: <xsl:value-of select="printdate"/>
                            Print time: <xsl:value-of select="printtime"/>
                        </fo:block>
                        <fo:block font-size="7px" padding-top="5px" border-top="1px solid black">TNT'S LIABILITY FOR LOSS DAMAGE AND DELAY IS LIMITED BY THE CMR CONVENTION OF THE WARSHAW CONVENTION, WHICHEVER IS APPLICABLE. THE SENDER AGREES THAT THE GENERAL CONDITIONS, ACCESIBLE VIA
                        THE TNT HELP, ARE ACCEPTABLE AND GOVERN THIS CONTRACT. IF NO SERVICE OR BILLING OPTION IS SELECTED THEN THE FASTEST AVAILABLE SERVICE WILL BE CHARGED TO THE SENDER</fo:block>
                    </fo:block-container>
                </fo:static-content>
                <fo:flow flow-name="xsl-region-body">
                    <fo:block-container font-size="10px">
                        <fo:block-container>
                            <fo:table>
                                <fo:table-column column-number="1" column-width="8%" />
                                <fo:table-column column-number="2" column-width="7%" /> 
                                <fo:table-column column-number="3" column-width="7%" /> 
                                <fo:table-column column-number="4" column-width="18%" />
                                <fo:table-column column-number="5" column-width="15%" />
                                <fo:table-column column-number="6" column-width="15%" />
                                <fo:table-column column-number="7" column-width="15%" />
                                <fo:table-column column-number="8" column-width="15%" />
                                <fo:table-header>
                                 <fo:table-row font-weight="bold">
                                     <fo:table-cell>
                                         <fo:block>Con Nr</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>No. of</fo:block>
                                         <fo:block>Pieces</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>Weight </fo:block>
                                         <fo:block>(kgs)</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>Shipper Ref.</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>Receiver</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>City</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>Destination</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell>
                                         <fo:block>Service</fo:block>
                                     </fo:table-cell>
                                 </fo:table-row>
                                </fo:table-header>
                                <fo:table-body font-size="8px">
                                    <xsl:for-each select="//consignment">
                                        <fo:table-row>
                                            <fo:table-cell>
                                                <fo:block><xsl:value-of select="number"/> </fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell text-align="center">
                                                <fo:block><xsl:value-of select="pieces"/> </fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell padding-right="5px" text-align="right">
                                                <fo:block><xsl:value-of select="weight"/> </fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell>
                                                <fo:block><xsl:value-of select="shipperref"/></fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell>
                                                <fo:block><xsl:value-of select="receiver"/></fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell>
                                                <fo:block><xsl:value-of select="city"/></fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell>
                                                <fo:block><xsl:value-of select="destination"/></fo:block>
                                            </fo:table-cell>
                                            <fo:table-cell>
                                                <fo:block><xsl:value-of select="service"/></fo:block>
                                            </fo:table-cell>
                                        </fo:table-row>
                                    </xsl:for-each>
                                    <fo:table-row >
                                    
                                    <fo:table-cell border-top="1px solid black" padding-top="10px" padding-bottom="3px" font-size="10px" number-columns-spanned="8">
                                        <fo:block font-style="italic">Account <xsl:value-of select="account/accountNumber"/> Totals:</fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell padding-bottom="10px" text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/consignments"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/pieces"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/weight"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell>
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell border-top="1px solid black" padding-top="10px" padding-bottom="3px" font-size="10px" number-columns-spanned="8">
                                            <fo:block font-style="italic">Sender <xsl:value-of select="sender/name"/> Totals:</fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell padding-bottom="10px" text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/consignments"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/pieces"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/weight"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell border-top="1px solid black" padding-top="10px" padding-bottom="3px" font-size="10px" number-columns-spanned="8">
                                            <fo:block font-style="italic">Grand totals:</fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell padding-bottom="10px" text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/consignments"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/pieces"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell text-align="center">
                                            <fo:block><xsl:value-of select="grandtotal/weight"/></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell >
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell border-top="1px solid black" padding-top="10px" padding-bottom="3px" font-size="10px" number-columns-spanned="8">
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                    </fo:table-row>
                                </fo:table-body>
                            </fo:table>
                        </fo:block-container>
                        <fo:block-container page-break-inside="avoid">
                            <fo:table>
                                <fo:table-column column-number="1" column-width="20%" />
                                <fo:table-column column-number="2" column-width="40%" />
                                <fo:table-column column-number="3" column-width="40%" />
                                <fo:table-body>
                                    <fo:table-row >
                                        <fo:table-cell padding-top="30px">
                                         <fo:block>Sender's Signature</fo:block>
                                     </fo:table-cell>
                                     <fo:table-cell border-bottom="1px solid black">
                                         <fo:block></fo:block>
                                     </fo:table-cell>
                                        <fo:table-cell padding-top="30px" padding-left="20px">
                                         <fo:block >
                                         Date ___ / ___ / _____
                                         </fo:block> 
                                     </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row >
                                        <fo:table-cell padding-top="30px">
                                            <fo:block>Received by TNT</fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell border-bottom="1px solid black">
                                            <fo:block></fo:block>
                                        </fo:table-cell>
                                        <fo:table-cell padding-top="30px" padding-left="20px">
                                            <fo:block >
                                                Date ___ / ___ / _____  Time ___ : ___ hrs
                                            </fo:block> 
                                        </fo:table-cell>
                                    </fo:table-row>
                                </fo:table-body>
                            </fo:table>
                        </fo:block-container>
                    </fo:block-container>
                </fo:flow>
                
            </fo:page-sequence>
           
        </fo:root>
    </xsl:template> 
</xsl:stylesheet>