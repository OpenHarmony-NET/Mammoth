using OpenHarmony.NDK.Bindings.Native;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mammoth
{
    public class Entry
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)], EntryPoint = "RegisterEntryModule")]
        public unsafe static void RegisterEntryModule()
        {
            napi_module demoModule = new()
            {
                nm_version = 1,
                nm_flags = 0,
                nm_filename = null,
                nm_modname = (sbyte*)Marshal.StringToHGlobalAnsi("mammoth"),
                nm_priv = null,
                napi_addon_register_func = &Init,
                reserved_0 = null,
                reserved_1 = null,
                reserved_2 = null,
                reserved_3 = null,
            };

            node_api.napi_module_register(&demoModule);
        }
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        public unsafe static napi_value Init(napi_env env, napi_value exports)
        {
            ArkTsLibrary.Init(env, exports);
            return exports;
        }
    }
}