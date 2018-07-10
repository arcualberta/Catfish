<?xml version="1.0"?>

<xsl:transform version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<fields>
		<xsl:for-each select="//metadata/metadata-set/fields/field">
			<field elementguid="{../../../../@guid}" metadatasetguid="{../../@guid}" guid="{@guid}" model-type="{@model-type}">
				<xsl:for-each select="value/text">
					<entry lang="{@xml:lang}">
						<name><xsl:value-of select="../../name/text[@xml:lang=current()/@xml:lang]"/></name>
						<value><xsl:value-of select="current()"/></value>
					</entry>
				</xsl:for-each>
			</field>
		</xsl:for-each>
	</fields>
</xsl:template>

</xsl:transform>