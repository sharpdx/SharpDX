namespace SharpDX.Direct2D1
{
    public partial interface CommandSink4
    {
        /// <summary>	
        /// <p>Sets a new primitive blend mode. Allows access to the MAX primitive blend mode.</p>	
        /// </summary>	
        /// <param name="primitiveBlend">The primitive blend that will apply to subsequent primitives.</param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1CommandSink4::SetPrimitiveBlend2']/*"/>	
        /// <msdn-id>mt797802</msdn-id>	
        /// <unmanaged>HRESULT ID2D1CommandSink4::SetPrimitiveBlend2([In] D2D1_PRIMITIVE_BLEND primitiveBlend)</unmanaged>	
        /// <unmanaged-short>ID2D1CommandSink4::SetPrimitiveBlend2</unmanaged-short>	
        void SetPrimitiveBlend2(PrimitiveBlend primitiveBlend);
    }
}