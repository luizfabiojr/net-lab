CREATE PROCEDURE [dbo].[P_TOTAL_POR_CFOP]
	@pCfop varchar(5) OUTPUT,
	@pTotalBaseICMS decimal(18,5) OUTPUT,
	@pTotalICMS decimal(18,5) OUTPUT,
	@pTotalBaseIPI decimal(18,5) OUTPUT,
	@pTotalIPI decimal(18,5) OUTPUT
AS
	SELECT 
		@pCfop = Cfop,
		@pTotalBaseICMS = Sum(BaseIcms),
		@pTotalICMS = Sum(ValorIcms),
		@pTotalBaseIPI = Sum(BaseIpi),
		@pTotalIPI = Sum(ValorIpi)
	from NotaFiscalItem
	where Cfop = @pCfop
	Group by Cfop	
RETURN 0
