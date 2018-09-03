echo "Start Building"
#project_path="xxxxxxxxx"
project_path=$1
ipa_filename=$2
scheme_name="Unity-iPhone" 
export_plist=${project_path}/ExportOptions.plist
project_name="Unity-iPhone.xcodeproj"
build_output_dir="build"
configuration="Release"
archivePath=${build_output_dir}/${scheme_name}.xcarchive
 
cd $project_path
 
echo "Clean Xcode"
echo ${ipa_filename}
echo $project_path/../${ipa_filename}.ipa 
echo $project_path/../$ipa_filename.ipa 
 
xcodebuild clean
 
xcodebuild \
archive -project "${project_name}" \
-scheme "${scheme_name}" \
-configuration "$configuration" \
-archivePath "${archivePath}"
 
xcodebuild \
-exportArchive -archivePath "${archivePath}" \
-exportOptionsPlist "${export_plist}" \
-exportPath "${build_output_dir}"
 

 if [ -e $project_path/$build_output_dir/$scheme_name.ipa ]; then
	cp $project_path/$build_output_dir/$scheme_name.ipa $project_path/../$ipa_filename.ipa 
fi

open ${project_path}
 
echo "Successfully exported and signed the ipa file"
