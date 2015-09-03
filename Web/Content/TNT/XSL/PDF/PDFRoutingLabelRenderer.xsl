<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                xmlns:url="http://whatever/java/java.net.URLEncoder"> 

<xsl:import href="PDFItalianDomesticRoutingLabelRenderer.xsl"/>
<xsl:import href="PDFFrenchDomesticRoutingLabelRenderer.xsl"/>
<xsl:import href="PDFRestOfWorldRoutingLabelRenderer.xsl"/>

<xsl:output method="xml" indent="yes" /> 

<xsl:param name="twoDbarcode_url" />
<xsl:param name="barcode_url" />
<xsl:param name="images_dir" />

<xsl:template match="/"> 
   <fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format"> 
      <fo:layout-master-set> 
         <fo:simple-page-master master-name="expressLabel"
                                page-height="15cm" page-width="10cm"
                                margin="0.2cm"> 
            <fo:region-body /> 
         </fo:simple-page-master> 
      </fo:layout-master-set> 
      <fo:page-sequence master-reference="expressLabel"> 

<!-- Page content goes here -->
         <fo:flow flow-name="xsl-region-body"> 

			<xsl:choose>
                <xsl:when test="(//consignmentLabelData/sender/country = 'IT') and (//consignmentLabelData/delivery/country = 'IT')">
                    <xsl:for-each select="//consignment/pieceLabelData"> 
                        <xsl:call-template name="ItalianDomesticPdf" />
                    </xsl:for-each> 
                </xsl:when>
                <xsl:when test="(//consignmentLabelData/sender/country = 'FR') and (//consignmentLabelData/delivery/country = 'FR')">
                    <xsl:for-each select="//consignment/pieceLabelData"> 
                        <xsl:call-template name="FrenchDomesticPdf" />
                    </xsl:for-each> 
                </xsl:when>
				<xsl:otherwise>
           			<xsl:for-each select="//consignment/pieceLabelData"> 
						<xsl:call-template name="RestOfWorldPdf">
							<xsl:with-param name="barcode_url" select = "$barcode_url" />
						</xsl:call-template>
           			</xsl:for-each> 
				</xsl:otherwise>
			</xsl:choose>

         </fo:flow> 
      </fo:page-sequence> 
   </fo:root> 
   
</xsl:template> 
 
</xsl:stylesheet>