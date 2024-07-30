sleep 10
echo 'ROOTUSER' | mc alias set myminio http://minio:9000 ${MINIO_ROOT_USER} ${MINIO_ROOT_PASS} 

echo 'ROOTUSER' | mc admin user add myminio ${MINIO_UPLOADER_USER} ${MINIO_UPLOADER_PASSWORD}
echo 'ROOTUSER' | mc admin user add myminio ${MINIO_PROCESSOR_USER} ${MINIO_PROCESSOR_PASSWORD}

echo 'ROOTUSER' | mc admin user svcacct add --access-key ${MINIO_PROCESSOR_USER_ACCESS} --secret-key ${MINIO_PROCESSOR_PASSWORD} myminio ${MINIO_PROCESSOR_USER}
echo 'ROOTUSER' | mc admin user svcacct add --access-key ${MINIO_UPLOADER_USER_ACCESS} --secret-key ${MINIO_UPLOADER_PASSWORD} myminio ${MINIO_PROCESSOR_USER}

mc mb myminio/landingimages 
mc mb myminio/images 
mc admin config set myminio notify_webhook:1 endpoint=$PROCESSING_WEBHOOK_ENDPOINT 
mc admin service restart myminio 

mc event add myminio/landingimages arn:minio:sqs::1:webhook --event put 

#mc anonymous set public myminio/landingimages  

mc anonymous set-json landingimagespolicy.json myminio/landingimages
mc anonymous set-json imagespolicy.json myminio/images


mc admin policy create myminio uploader-policy uploader.json
mc admin policy create myminio processor-policy processor.json

mc admin policy attach myminio uploader-policy --user $MINIO_UPLOADER_USER
mc admin policy attach myminio processor-policy --user $MINIO_PROCESSOR_USER

echo 'finished'