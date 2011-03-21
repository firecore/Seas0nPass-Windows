using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seas0nPass.Models
{
    public class Patch_421_8C154 : IPatch
    {
        private string currentMessage;
        private void UpdateCurrentMessage(string message)
        {
            currentMessage = message;
            if (CurrentMessageChanged != null)
                CurrentMessageChanged(this, EventArgs.Empty);
        }

        private int progress;
        private void UpdateProgress(int progress)
        {
            this.progress = progress;
            if (CurrentProgressChanged != null)
                CurrentProgressChanged(this, EventArgs.Empty);
        }


        public string PerformPatch()
        {

            UpdateCurrentMessage("Unzipping...");

            Directory.SetCurrentDirectory(Utils.WORKING_FOLDER);

            Utils.RecreateDirectory(Utils.UNZIP_FOLDER_PATH);
            ArchiveUtils.ExtractZipFile(Utils.DOWNLOADED_FILE_PATH, null, Utils.UNZIP_FOLDER_PATH);

            UpdateProgress(5);

            UpdateCurrentMessage("Decrypting ramdisk...");
            Utils.RecreateDirectory(Utils.IPSW_FOLDER_PATH);
            Utils.RecreateDirectory(Utils.TMP_FOLDER_PATH);
            Utils.RecreateDirectory(Utils.OUTPUT_FOLDER_NAME);

            Utils.ExecuteResource("_421_010_decrypt_ramdisk");

            UpdateProgress(6);

            Utils.RecreateDirectory(Utils.TMP_FOLDER_PATH);

            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), Utils.IPSW_FOLDER_PATH, Utils.DMG_FILE_NAME),
                      Path.Combine(Directory.GetCurrentDirectory(), Utils.TMP_FOLDER_PATH, Utils.OUR_DMG_FILE_NAME));


            UpdateCurrentMessage("Patching ramdisk...");
            Utils.ExecuteResource("_421_020_patch_asr");

            UpdateProgress(8);

            UpdateCurrentMessage("Encrypting ramdisk...");
            Utils.ExecuteResource("_421_030_encrypt_ramdisk");

            UpdateProgress(9);

            UpdateCurrentMessage("Decrypting file system...");
            Utils.ExecuteResource("_421_040_decrypt_filesystem");

            UpdateProgress(17);

            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), Utils.IPSW_FOLDER_PATH, Utils.ANOTHER_DMG_FILE_NAME),
                      Path.Combine(Directory.GetCurrentDirectory(), Utils.TMP_FOLDER_PATH, Utils.OUR_BIG_DMG_FILE_NAME));

            Utils.ExecuteResource("_421_050_unpack_filesystem_image");

            UpdateProgress(49);

            UpdateCurrentMessage("Patching file system...");

            Utils.ExecuteResource("_421_060_patch_fstab");

            UpdateProgress(54);

            Utils.ExecuteResource("_421_061_patch_appletv");

            UpdateProgress(55);

            Utils.ExecuteResource("_421_062_patch_Services_plist");

            Utils.ExecuteResource("_421_063_add_hfs_mdb");

            Utils.ExecuteResource("_421_064_add_kern_sploit");

            Utils.ExecuteResource("_421_065_add_punchd");

            UpdateProgress(56);

            UpdateCurrentMessage("Installing software...");

            Utils.RecreateDirectory(Utils.CYDIA_FOLDER);

            ArchiveUtils.ExtractGZip(Utils.CYDIA_ARCHIVE_NAME, Utils.CYDIA_FOLDER, Utils.CYDIA_EXTRACTED_NAME);

            UpdateProgress(59);

            Utils.ExecuteResource("_421_080_add_cydia_to_image");

            UpdateProgress(177);

            Utils.ExecuteResource("_421_081_create_symlinks");

            Utils.ExecuteResource("_421_082_add_uncompress");

            UpdateProgress(179);

            UpdateCurrentMessage("Creating IPSW...");

            Utils.ExecuteResource("_421_090_pack_filesystem_image");



            File.Copy(Path.Combine(Utils.UNZIP_FOLDER_PATH, Utils.KERNEL_CACHE_FILE_NAME),
                      Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.KERNEL_CACHE_FILE_NAME));


            File.Copy(Path.Combine(Utils.UNZIP_FOLDER_PATH, Utils.BUILD_MANIFEST_FILE_NAME),
                      Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.BUILD_MANIFEST_FILE_NAME));

            File.Copy(Path.Combine(Utils.UNZIP_FOLDER_PATH, Utils.RESTORE_FILE_NAME),
                      Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.RESTORE_FILE_NAME));

            Utils.RecreateDirectory(Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME));

            Utils.CopyDirectory(Path.Combine(Utils.UNZIP_FOLDER_PATH, Utils.FIRMWARE_FOLDER_NAME),
                                Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME));

            Utils.ExecuteResource("_421_110_patch_dfu");

            UpdateProgress(229);

            UpdateCurrentMessage("Compressing IPSW...");

            File.Delete(Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.IBSS_FILE_NAME));
            File.Delete(Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.PATCHED_DFU_FILE_NAME));
            File.Delete(Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.DECRYPTED_DFU_FILE_NAME));

            File.Move(Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.ENCRYPTED_DFU_FILE_NAME),
                      Path.Combine(Utils.OUTPUT_FOLDER_NAME, Utils.FIRMWARE_FOLDER_NAME, Utils.DFU_FOLDER_NAME, Utils.IBSS_FILE_NAME));

            var fullOutputFileName = Path.Combine(Directory.GetCurrentDirectory(), Utils.OUTPUT_FIRMWARE_NAME);

            Directory.SetCurrentDirectory(Utils.OUTPUT_FOLDER_NAME);

            ArchiveUtils.CreateSample(fullOutputFileName, null, Utils.GetAllFileInFodler(Directory.GetCurrentDirectory()));

            UpdateProgress(300);

            return fullOutputFileName;
            
        }

        public string CurrentMessage
        {
            get { return currentMessage; }
        }

        public event EventHandler CurrentMessageChanged;


        public int CurrentProgress
        {
            get { return progress; }
        }

        public event EventHandler CurrentProgressChanged;
    }
}
