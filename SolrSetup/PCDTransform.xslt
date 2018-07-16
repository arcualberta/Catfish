<?xml version="1.0"?>

<xsl:transform version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<fields guid="{//@guid}">
		<xsl:for-each select="//metadata/metadata-set/fields">
			<xsl:for-each select="field|Field">
				<field elementguid="{../../../../@guid}" metadatasetguid="{../../@guid}" guid="{@guid}" model-type="{@model-type}">
					<xsl:for-each select="value/text">
						<entry lang="{@xml:lang}">
							<name><xsl:value-of select="../../name/text[@xml:lang=current()/@xml:lang]"/></name>
							<value><xsl:value-of select="current()"/></value>
							<guid><xsl:value-of select="../../../../@guid"/>_<xsl:value-of select="../../@guid"/>_</guid>
							<model-type><xsl:value-of select="../../@model-type"/></model-type>
						</entry>
					</xsl:for-each>
					<xsl:for-each select="options/option[@selected='true']/text">
						<option lang="{@xml:lang}">
							<guid><xsl:value-of select="../../../../../@guid"/>_<xsl:value-of select="../../../@guid"/>_</guid>
							<name><xsl:value-of select="../../../name/text[@xml:lang=current()/@xml:lang]"/></name>
							<value><xsl:value-of select="current()"/></value>
						</option>
					</xsl:for-each>
				</field>
			</xsl:for-each>
		</xsl:for-each>
	</fields>
</xsl:template>

</xsl:transform>