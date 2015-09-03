<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"> 

<xsl:template name="FrenchDomesticPdf">

	<fo:block-container break-before="page" 
	                    border-style="solid"
	                    border-width="1"
	                    border-collapse="collapse"
	                    font-family="sans-serif"
	                    top="0px"
	                    left="0px"
	                    width="270px"
	                    height="405px"> 

		<!-- Logo-->
		<fo:block-container absolute-position="absolute" 
			                top="5px" 
			                left="3px">
			<fo:block>
			    <fo:external-graphic src="url('{$images_dir}/logo.jpg')"
			                         content-height="25px"/>
			</fo:block>
		</fo:block-container>
				
		<!-- Contact Details -->       
		<fo:block-container
		          absolute-position="absolute" 
		          top="0px"
		          left="91px"
		          height="50px" 
		          width="211px">  
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"
			          padding-top="3px"> 
			   <fo:inline padding-left="1px">
			        <xsl:text>Service Client</xsl:text>
			   </fo:inline>
			</fo:block>
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"> 
			   <fo:inline padding-left="1px">
			        <xsl:text>Fax</xsl:text>
			   </fo:inline>
			</fo:block>
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"> 
			   <fo:inline padding-left="1px">
			        <xsl:text>Web</xsl:text>
			   </fo:inline>
			</fo:block>
		</fo:block-container>
		
		<fo:block-container
		          absolute-position="absolute" 
		          top="0px"
		          left="142px"
		          height="50px" 
		          width="181px">  
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"
			          padding-top="3px"> 
			   <fo:inline>
			        <xsl:text> : +33(0)825 033 033</xsl:text>
			   </fo:inline>
			</fo:block>
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"> 
			   <fo:inline>
			        <xsl:text> : +33(0)825 031 021</xsl:text>
			   </fo:inline>
			</fo:block>
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"> 
			   <fo:inline>
			        <xsl:text> : www.tnt.fr</xsl:text>
			   </fo:inline>
			</fo:block>
		</fo:block-container>
		
		
		<!-- Legal Comments -->       
		<fo:block-container
		          absolute-position="absolute" 
		          top="29px"
		          left="91px"
		          height="23px" 
		          width="211px">  
			<fo:block font-weight="normal"
			          font-size="5pt"
			          text-align="left"
			          padding-top="4px"> 
			   <xsl:value-of select="../consignmentLabelData/legalComments" />
			</fo:block>
		</fo:block-container>
		
		<!-- Con Number -->
		<fo:block-container absolute-position="absolute"
		                    top="44px"
		                    left="4px"
		                    width="131px"
		                    height="28px"
		                    line-height="80%"
		                    padding-left="3px"
		                    padding-top="1px"
		                    background-color="#FFFFFF"
		                    border-collapse="collapse"
		                    border-right-width="1px"
		                    border-right-style="none"
		                    border-right-color="black"
		                    border-top-width="1px"
		                    border-top-style="none"
		                    border-top-color="black"
		                    border-bottom-width="1px"
		                    border-bottom-style="none"
		                    border-bottom-color="#FFFFFF">
			<fo:block font-weight="bold"
			          font-size="14pt"
			          line-height="100%"> 
			   <xsl:value-of select="../consignmentLabelData/consignmentNumber" />                   
			</fo:block> 
		</fo:block-container>
		
		<!-- Service -->
		<fo:block-container absolute-position="absolute"
		                    top="42px"
		                    left="136px"
		                    width="134px"
		                    height="18px"
		                    line-height="70%"
		                    padding-top="2px"
		                    padding-left="2px"
		                    background-color="#000000"
		                    border-color="black"
		                    border-top-width="1px"
		                    border-top-style="none">
		
		    <xsl:choose>
		        <xsl:when
		            test="string-length(../consignmentLabelData/product)>12">
		
		            <fo:block font-size="14pt"
		                font-weight="bold" color="#FFFFFF"
		                   line-height="100%">
		                <xsl:value-of
		                    select="../consignmentLabelData/product" />
		
		            </fo:block>
		        </xsl:when>
		        <xsl:otherwise>
		
		            <fo:block font-size="16pt"
		                font-weight="bold" color="#FFFFFF"
		                   line-height="100%">
		                <xsl:value-of
		                    select="../consignmentLabelData/product" />
		
		            </fo:block>
		        </xsl:otherwise>
		    </xsl:choose>
		</fo:block-container>
		
		<!-- Options-->
		<fo:block-container absolute-position="absolute"
		                    top="62px"
		                    left="136px"
		                    width="134px"
		                    height="14px"
		                    padding-left="2px"
		                    background-color="#000000"
		                    border-color="black"
		                    border-bottom-width="1px"
		                    border-bottom-style="none">
		
		<xsl:variable name="numberOptions"
		              select="count(../consignmentLabelData/option)" />
		
		<xsl:choose>
		   <xsl:when
		       test="$numberOptions >1">
		       <fo:block font-size="12pt"
		           padding-top="2px" font-weight="bold"
		           color="#FFFFFF"
		           line-height="95%">
		           <xsl:for-each
		               select="../consignmentLabelData/option">
		               <fo:inline>
		                   <xsl:value-of
		                       select="@id" />
		                   <xsl:text>  </xsl:text>
		               </fo:inline>
		           </xsl:for-each>
		       </fo:block>
		   </xsl:when>
		   <xsl:otherwise>
		       <xsl:choose>
		           <xsl:when test="string-length(../consignmentLabelData/option)>19">
		               <fo:block
		                   font-size="10pt" padding-top="2px"
		                   text-align="left"
		                   font-weight="bold"
		                   color="#FFFFFF"
		                   line-height="95%">
		                   <xsl:value-of select="../consignmentLabelData/option" />
		               </fo:block>
		           </xsl:when>
		           <xsl:otherwise>
		               <fo:block
		                   font-size="12pt" padding-top="2px"
		                   text-align="left"
		                   font-weight="bold"
		                   color="#FFFFFF"
		                   line-height="95%">
		                   <xsl:value-of select="../consignmentLabelData/option" />
		               </fo:block>
		           </xsl:otherwise>
		       </xsl:choose>
		
		   </xsl:otherwise>
		</xsl:choose>
		</fo:block-container>
		
		
		<!-- Piece-->
		<fo:block-container absolute-position="absolute"
		                    top="62px"
		                    left="3px"
		                    width="68px"
		                    height="25px"
		                    line-height="75%"
		                    padding-left="3px"
		                    border-color="black"
		                    border-bottom-width="1px"
		                    border-bottom-style="none">
			<fo:block font-weight="bold"
			          font-size="12pt"
			          line-height="100%"> 
			   <fo:inline><xsl:value-of select="pieceNumber" />
			      sur 
			      <xsl:value-of select="../consignmentLabelData/totalNumberOfPieces" /></fo:inline>
			   
			</fo:block> 
		</fo:block-container>
		           
		<!-- Weight -->
		<fo:block-container absolute-position="absolute"
		                    top="62px"
		                    left="75px"
		                    height="25px"
		                    line-height="75%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none"
		                    border-bottom-width="1px"
		                    border-bottom-style="none">
		<fo:block font-weight="bold"
		          font-size="12pt"
		          line-height="100%"> 
		    <xsl:choose>
		        <xsl:when test="weightDisplay/@renderInstructions='highlighted'">
		           <fo:inline background-color="black" color="white">
		               <xsl:value-of select="weightDisplay" />
		           </fo:inline>
		        </xsl:when>
		        <xsl:otherwise>
		           <fo:inline>
		               <xsl:value-of select="weightDisplay" />
		           </fo:inline>
		        </xsl:otherwise>
		    </xsl:choose>
		</fo:block>
		</fo:block-container>
		
		<!-- Customer Reference Barcode -->
		<fo:block-container absolute-position="absolute"
		                    top="77px"
		                    left="60px"
		                    width="200px">
			<fo:block text-align="right">
			    <fo:external-graphic content-width="200px" content-height="18px" scaling="non-uniform">
					<xsl:if test="string-length(barcodeForCustomer) > 0">
					   <xsl:attribute name="src">
					       url('<xsl:value-of select="concat($barcode_url,barcodeForCustomer)" />')
					    </xsl:attribute>
					</xsl:if>
			     </fo:external-graphic>
			 </fo:block>
		 </fo:block-container> 
		 
		 <!-- Customer Reference-->
		<fo:block-container absolute-position="absolute"
		                    top="102px"
		                    left="3px"
		                    width="131px"
		                    height="19px"
		                    padding-top="1px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px">Ref:</fo:block>
		</fo:block-container> 
		
		<fo:block-container absolute-position="absolute"
		                    top="102px"
		                    left="23px"
		                    width="131px"
		                    height="19px"
		                    padding-top="1px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px" font-weight="bold"> 
		       <xsl:value-of select="pieceReference" /> 
		    </fo:block> 
		</fo:block-container> 
		
		<!-- Account -->
		<fo:block-container absolute-position="absolute"
		                    top="112px"
		                    left="3px"
		                    width="131px"
		                    height="19px"
		                    padding-top="1px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px">Cpt:</fo:block>                                 
		</fo:block-container> 
		
		<fo:block-container absolute-position="absolute"
		                    top="112px"
		                    left="23px"
		                    width="131px"
		                    height="19px"
		                    padding-top="1px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px" font-weight="bold"> 
		       <xsl:value-of select="../consignmentLabelData/account/accountNumber" /> 
		    </fo:block> 
		</fo:block-container> 
		
		<!-- Delivery Depot -->
		<fo:block-container absolute-position="absolute"
		                    top="113px"
		                    left="157px"> 
		    <fo:block font-size="42px">
				<xsl:if test="string-length(../consignmentLabelData/delivery/postcode) >= 2">
				<xsl:value-of select="substring(../consignmentLabelData/delivery/postcode,1,2)"/>
				</xsl:if>
           </fo:block>
       </fo:block-container>
		
		<!-- Sender Address-->
		<fo:block-container absolute-position="absolute"
		                    top="122px"
		                    left="3px"
		                    width="131px"
		                    height="55px"
		                    padding-left="3px"
		                    padding-top="1px"
		                    border-width="1px"
		                    border-top-style="none"
		                    border-left-style="none"
		                    border-right-style="none"> 
		    <fo:block font-size="7px">Exp:</fo:block>
		</fo:block-container>
		
		<fo:block-container absolute-position="absolute"
		                    top="122px"
		                    left="23px"
		                    width="131px"
		                    height="55px"
		                    padding-left="3px"
		                    padding-top="1px"
		                    border-width="1px"
		                    border-top-style="none"
		                    border-left-style="none"
		                    border-right-style="none"> 
		    <fo:block font-size="7px"> 
		        <xsl:if test="string-length(../consignmentLabelData/sender/name) > 0">
		           <xsl:value-of select="../consignmentLabelData/sender/name" />
		        </xsl:if>
		    </fo:block> 
		    <fo:block font-size="7px"> 
		        <xsl:if test="string-length(../consignmentLabelData/sender/addressLine1) > 0">
		           <xsl:value-of select="../consignmentLabelData/sender/addressLine1" />
		        </xsl:if>
		    </fo:block> 
		    <fo:block font-size="7px"> 
		        <xsl:if test="string-length(../consignmentLabelData/sender/addressLine2) > 0">
		           <xsl:value-of select="../consignmentLabelData/sender/addressLine2" />
		        </xsl:if>
		    </fo:block> 
		    <fo:block font-size="7px"> 
		       <xsl:value-of select="../consignmentLabelData/sender/town" /><xsl:text>  </xsl:text>
		       
		       <xsl:value-of select="../consignmentLabelData/sender/postcode" /><xsl:text>  </xsl:text>
		    </fo:block> 
		    <fo:block font-size="7px"> 
		       <xsl:value-of select="../consignmentLabelData/sender/country" />
		    </fo:block> 
		</fo:block-container> 
		
		<!-- Delivery Address-->
		<fo:block-container absolute-position="absolute"
		                    top="167px"
		                    left="3px"
		                    width="250px"
		                    height="55px"
		                    padding-left="3px"
		                    padding-top="1px"
		                    border-width="1px"
		                    border-bottom-style="none"
		                    border-left-style="none"
		                    border-right-style="none">
		    <fo:block font-size="7px">Dest:</fo:block>
		</fo:block-container>
		
		<fo:block-container absolute-position="absolute"
		                    top="167px"
		                    left="23px"
		                    width="250px"
		                    height="70px"
		                    line-height="110%"
		                    padding-left="3px"
		                    padding-top="1px"
		                    border-width="1px"
		                    border-bottom-style="none"
		                    border-left-style="none"
		                    border-right-style="none">
		    <xsl:if test="string-length(../consignmentLabelData/delivery/name) > 0">
		        <fo:block font-size="14px">
		           <xsl:value-of select="../consignmentLabelData/delivery/name" />
		        </fo:block> 
		    </xsl:if>
		    <fo:block font-size="14px"> 
		        <xsl:if test="string-length(../consignmentLabelData/delivery/addressLine1) > 0">
		           <xsl:value-of select="../consignmentLabelData/delivery/addressLine1" />
		        </xsl:if>
		    </fo:block> 
		    <xsl:if test="string-length(../consignmentLabelData/delivery/addressLine2) > 0">
		    <fo:block font-size="14px"> 
		           <xsl:value-of select="../consignmentLabelData/delivery/addressLine2" />
		    </fo:block> 
		    </xsl:if>
		    <fo:block font-size="14px"> 
		       <xsl:value-of select="../consignmentLabelData/delivery/town" /><xsl:text>  </xsl:text>
		       <xsl:value-of select="../consignmentLabelData/delivery/postcode" />
		    </fo:block> 
		    <fo:block font-size="14px"> 
		       <xsl:value-of select="../consignmentLabelData/delivery/country" />
		    </fo:block> 
		</fo:block-container> 
		
		<!-- Special Instructions -->       
		<fo:block-container
		          absolute-position="absolute" 
		          top="232px"
		          left="3px"
		          height="23px" 
		          width="320px">  
			<fo:block font-weight="normal"
			          font-size="8pt"
			          text-align="left"
			          padding-left="3px"> 
			   <xsl:value-of select="substring(../consignmentLabelData/specialInstructions,1,65)" />
			</fo:block>
		</fo:block-container>
		
		<!-- Contact Name -->
		<fo:block-container absolute-position="absolute"
		                    top="242px"
		                    left="3px"
		                    width="60px"
		                    height="23px"
		                    padding-top="0px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px">Contact Name:</fo:block>
		</fo:block-container> 
		
		<fo:block-container absolute-position="absolute"
		                    top="242px"
		                    left="65px"
		                    width="131px"
		                    height="23px"
		                    padding-top="1px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px" font-weight="bold"> 
				<xsl:if test="string-length(../consignmentLabelData/contact) > 0">
				    <xsl:value-of select="../consignmentLabelData/contact/name" />
				</xsl:if>
		    </fo:block> 
		</fo:block-container>
		          
        <!-- Contact Phone -->
		<fo:block-container absolute-position="absolute"
		                    top="252px"
		                    left="3px"
		                    width="60px"
		                    height="23px"
		                    padding-top="0px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px">Contact Phone:</fo:block>
		</fo:block-container> 
		
		<fo:block-container absolute-position="absolute"
		                    top="252px"
		                    left="65px"
		                    width="131px"
		                    height="23px"
		                    padding-top="0px"
		                    line-height="70%"
		                    border-color="black"
		                    border-right-width="1px"
		                    border-right-style="none">
		    <fo:block font-size="8px" font-weight="bold">  
		        <xsl:if test="string-length(../consignmentLabelData/contact) > 0">
		            <xsl:value-of select="../consignmentLabelData/contact/telephoneNumber" />
		        </xsl:if>
		    </fo:block> 
		</fo:block-container>  
		
		<!-- Postcode / Cluster code-->
		<fo:block-container absolute-position="absolute"
		                    top="267px"
		                    left="3px"
		                    width="60px"
		                    height="28px"
		                    padding-left="3px"
		                    border-width="1px"
		                    border-bottom-style="none">
			<fo:block font-size="8pt">
			    Code Postale /
			</fo:block>
			<fo:block font-size="8pt">
			    Code Satellite
			</fo:block>
		</fo:block-container>
		
		<!-- Postcode / Cluster Detail -->
		<fo:block-container absolute-position="absolute"
		                    top="263px"
		                    left="62px"
		                    width="70px"
		                    height="24px"
		                    line-height="90%"
		                    padding-left="2px"
		                    background-color="black"
		                    border-width="1px"
		                    border-right-style="none"
		                    border-bottom-style="none">

	        <fo:block padding-top="9px"
	                  padding-left="2px"
	                  font-weight="bold"
	                  font-size="20px" 
	                  color="#FFFFFF">
	            <xsl:value-of select="../consignmentLabelData/delivery/postcode"/>
	        </fo:block>
		</fo:block-container>
		
		
		<!-- Pickup Date-->
		<fo:block-container absolute-position="absolute"
		                    top="264px"
		                    left="138px"
		                    width="150px"
		                    height="25px">
			<fo:block font-size="8px" padding-left="3px">
			    <fo:inline>
			        Date Ramassage:
			        <xsl:call-template name="FormatDateFrenchDomestic">    
			            <xsl:with-param name="DateTime" select="../consignmentLabelData/collectionDate"/> 
			        </xsl:call-template> 
			    </fo:inline>                                     
			</fo:block> 
		</fo:block-container> 
		
		
		<!-- Cash Amount -->
		<fo:block-container absolute-position="absolute"
		                    top="274px"
		                    left="138px"
		                    width="150px"
		                    height="25px">
			<fo:block font-size="14px" padding-left="3px">
			    <fo:inline>
			        <xsl:for-each select="../consignmentLabelData/option">
			            <xsl:if test="@id='CO' or @id='RP'">
			            <xsl:value-of select="../cashAmount"/>
			         </xsl:if>
			        </xsl:for-each>
			    </fo:inline>                                     
			</fo:block> 
		</fo:block-container> 
		
		<!-- Barcode-->
		<fo:block-container absolute-position="absolute"
		                    top="292px"
		                    left="0px"
		                    width="270px">
			 <fo:block text-align="center">
			     <fo:external-graphic content-width="260px" content-height="95px" scaling="non-uniform">
			          <xsl:attribute name="src">
			            url('<xsl:value-of select="concat($barcode_url,barcode)" />')
			          </xsl:attribute> 
			     </fo:external-graphic>  
			 </fo:block>
		</fo:block-container> 
		<fo:block-container absolute-position="absolute"
		                    top="394px"
		                    left="0px"
		                    width="270px">
			 <fo:block font-size="8px" text-align="center">
			    <xsl:value-of select="barcode" />
			</fo:block>
		</fo:block-container> 
	                
	</fo:block-container>  
   
</xsl:template> 

<xsl:template name="FormatDateFrenchDomestic">
    <!-- expected date format 2008 06 16 -->
    <xsl:param name="DateTime" />
    <!-- new date format 20 June 2007 -->
    <xsl:variable name="year">
        <xsl:value-of select="substring-before($DateTime,'-')" />
    </xsl:variable>
    <xsl:variable name="mo-temp">
        <xsl:value-of select="substring-after($DateTime,'-')" />
    </xsl:variable>
    <xsl:variable name="mo">
        <xsl:value-of select="substring-before($mo-temp,'-')" />
    </xsl:variable>
    <xsl:variable name="day">
        <xsl:value-of select="substring-after($mo-temp,'-')" />
    </xsl:variable>

    <xsl:value-of select="$day" />
    <xsl:text> </xsl:text>
    <xsl:choose>
        <xsl:when test="$mo = '1' or $mo = '01'">Jan</xsl:when>
        <xsl:when test="$mo = '2' or $mo = '02'">Fevr</xsl:when>
        <xsl:when test="$mo = '3' or $mo = '03'">Mar</xsl:when>
        <xsl:when test="$mo = '4' or $mo = '04'">Avr</xsl:when>
        <xsl:when test="$mo = '5' or $mo = '05'">Mai</xsl:when>
        <xsl:when test="$mo = '6' or $mo = '06'">Juin</xsl:when>
        <xsl:when test="$mo = '7' or $mo = '07'">Juil</xsl:when>
        <xsl:when test="$mo = '8' or $mo = '08'">Aout</xsl:when>
        <xsl:when test="$mo = '9' or $mo = '09'">Sept</xsl:when>
        <xsl:when test="$mo = '10'">Oct</xsl:when>
        <xsl:when test="$mo = '11'">Nov</xsl:when>
        <xsl:when test="$mo = '12'">Dec</xsl:when>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:value-of select="$year" />
</xsl:template>
 
</xsl:stylesheet>
