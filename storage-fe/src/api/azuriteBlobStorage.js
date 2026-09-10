import { BlobServiceClient } from "@azure/storage-blob";

// azurite 에뮬레이터 설정
const AZURITE_CONNECTION_STRING =
  "DefaultEndpointsProtocal=http;AccountName=devstoreaccount1" +
  import.meta.env.AZURITE_CONNECTION_STRING +
  "BlobEndpoint=http://127.0.0.1:10000/devstoraccount1";

async function main() {
  const blobServiceClient = BlobServiceClient.fromConnectionString(
    AZURITE_CONNECTION_STRING,
  );
  const containerName = "local--azurite-1";

  const containerClient = blobServiceClient.getContainerClient(containerName);
  await containerClient.createIfNotExists();

  console.log(`container : ${containerClient} >> created`);
}

main().catch(console.error);
