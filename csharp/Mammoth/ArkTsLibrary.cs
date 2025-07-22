using OpenHarmony.NDK.Bindings.Native;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Mammoth
{
    public unsafe static class ArkTsLibrary
    {
        public unsafe static napi_value Init(napi_env env, napi_value exports)
        {
            var methodNames = new[]{
                Marshal.StringToHGlobalAnsi("convertToHtml"),
                Marshal.StringToHGlobalAnsi("extractRawText")
            };
            napi_property_descriptor[] desc = [
                new (){ utf8name = (sbyte*)methodNames[0], name = default, method = &ConvertToHtml, getter = null, setter = null, value = default,  attributes = napi_property_attributes.napi_default, data = null },
                new (){ utf8name = (sbyte*)methodNames[1], name = default, method = &ExtractRawText, getter = null, setter = null, value = default,  attributes = napi_property_attributes.napi_default, data = null },
                ];
            fixed (napi_property_descriptor* p = desc)
                node_api.napi_define_properties(env, exports, 2, p);
            foreach (var methodName in methodNames)
                Marshal.FreeHGlobal(methodName);
            return exports;
        }
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static unsafe napi_value ConvertToHtml(napi_env env, napi_callback_info info)
        {
            ulong argc = 2;
            napi_value[] args = [default, default];
            fixed (napi_value* p = args)
                node_api.napi_get_cb_info(env, info, &argc, p, null, null);
            napi_valuetype valuetype0;
            node_api.napi_typeof(env, args[0], &valuetype0);
            sbyte* path = stackalloc sbyte[256];
            ulong length;
            node_api.napi_get_value_string_utf8(env, args[0], path, 256, &length);
            bool with_img;
            node_api.napi_get_value_bool(env, args[1], &with_img);
            napi_value result;
            var html = ExportFunction.ConvertToHtml(path, with_img);
            var messageBytes = Encoding.UTF8.GetBytes(html);
            fixed (void* p = messageBytes)
                node_api.napi_create_string_utf8(env, (sbyte*)p, (ulong)messageBytes.Length, &result);
            return result;
        }
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static unsafe napi_value ExtractRawText(napi_env env, napi_callback_info info)
        {
            ulong argc = 1;
            napi_value[] args = [default, default];
            fixed (napi_value* p = args)
                node_api.napi_get_cb_info(env, info, &argc, p, null, null);
            napi_valuetype valuetype0;
            node_api.napi_typeof(env, args[0], &valuetype0);
            sbyte* path = stackalloc sbyte[256];
            ulong length;
            node_api.napi_get_value_string_utf8(env, args[0], path, 256, &length);
            napi_value result;
            var html = ExportFunction.ExtractRawText(path);
            var messageBytes = Encoding.UTF8.GetBytes(html);
            fixed (void* p = messageBytes)
                node_api.napi_create_string_utf8(env, (sbyte*)p, (ulong)messageBytes.Length, &result);
            return result;
        }
    }
}