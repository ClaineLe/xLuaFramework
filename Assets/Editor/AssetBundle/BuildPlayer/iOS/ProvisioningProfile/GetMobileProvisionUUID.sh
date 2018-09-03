#!/bin/bash
if [ $# -ne 1 ]
then
echo "Usage: getmobileuuid the-mobileprovision-file-path"
exit 1
fi

mobileprovision_uuid=`/usr/libexec/PlistBuddy -c "Print UUID" /dev/stdin <<< $(/usr/bin/security cms -D -i $1)`
echo "UUID is:"${mobileprovision_uuid}
