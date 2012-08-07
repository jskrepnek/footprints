using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using footprints.worker;
using NUnit.Framework;
using Rhino.Mocks;

namespace footprints.test
{
    [TestFixture]
    public class FootprintsWorkerTests
    {
        IQueue Queue { get; set; }
        IStorage Storage { get; set; }
        IKeyReader Reader { get; set; }

        const string fakeQueueMessage = "{\"FirstName\":\"Joel\",\"LastName\":\"Skrepnek\",\"Phrase\":\"see if you can read this JSON\",\"Date\":\"2012-08-07T08:04:04.8787972-07:00\"}";
        const string fakeStorageText = "{\"List\":[{\"FirstName\":\"Joel\",\"LastName\":\"Skrepnek\",\"Phrase\":\"see if you can read this JSON\",\"Date\":\"2012-08-07T08:04:04.8787972-07:00\"}]}";
        const string fakeInvalidQueueMessage = "{:\"John\",\"LastName\":\"Doe\",\"Phrase\":\"this is a test message\",\"Date\":\"2012-08-07T08:04:04.8787972-07:00\"}";

        const string MSG_ID = "123456789";
        const string POP_RECEIPT = "asdfghjkl";

        [SetUp]
        public void Setup()
        {
            Queue = MockRepository.GenerateMock<IQueue>();
            Storage = MockRepository.GenerateMock<IStorage>();
            Reader = MockRepository.GenerateMock<IKeyReader>();
        }

        [TearDown]
        public void Teardown()
        {
            Queue = null;
            Storage = null;
            Reader = null;
        }

        void Act()
        {
            FootPrintsWorker.Execute(Queue, Storage, Reader);
        }

        [Test]
        public void Worker_NoMessage_ShouldNotCallUploadTextOnStorage()
        {
            // arrange
            String msgId;
            String popReceipt;

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(String.Empty)
                .OutRef(String.Empty, String.Empty);

            // act
            Act();

            // assert
            Reader.VerifyAllExpectations();
            Queue.VerifyAllExpectations();

            Storage.AssertWasNotCalled(storage => storage.UploadText(Arg<string>.Is.Anything));
        }

        [Test]
        public void Worker_QueueHasMessage_ChecksIfStorageExists()
        {
            // arrange
            String msgId;
            String popReceipt;

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(fakeQueueMessage);

            Storage.Expect(storage => storage.Exists())
                .Return(false);

            // act
            Act();

            // assert
            Reader.VerifyAllExpectations();
            Queue.VerifyAllExpectations();
            Storage.VerifyAllExpectations();
        }

        [Test]
        public void Worker_QueueHasInvalidMessage_DoesNotProcess()
        {
            String msgId;
            String popReceipt;

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(fakeInvalidQueueMessage)
                .OutRef(MSG_ID, POP_RECEIPT);

            Queue.Expect(queue => queue.DeleteMessage(Arg<string>.Is.Equal(MSG_ID), Arg<string>.Is.Equal(POP_RECEIPT)));

            Act();

            Reader.VerifyAllExpectations();
            Queue.VerifyAllExpectations();

            Storage.AssertWasNotCalled(storage => storage.Exists());
            Storage.AssertWasNotCalled(storage => storage.DownloadText());
            Storage.AssertWasNotCalled(storage => storage.UploadText(Arg<string>.Is.Anything));
        }

        [Test]
        public void Worker_QueueHasMessageNoExistingStorage_UploadsSerializedDataToStorage()
        {
            String msgId = String.Empty;
            String popReceipt = String.Empty;

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(fakeQueueMessage)
                .OutRef(MSG_ID, POP_RECEIPT);

            Queue.Expect(queue => queue.DeleteMessage(Arg<string>.Is.Equal(MSG_ID), Arg<string>.Is.Equal(POP_RECEIPT)));

            Storage.Expect(storage => storage.Exists())
                .Return(false);

            Storage.Expect(storage => storage.UploadText(Arg<string>.Is.Equal(fakeStorageText)));

            Act();

            Reader.VerifyAllExpectations();
            Queue.VerifyAllExpectations();
            Storage.VerifyAllExpectations();
        }

        [Test]
        public void Worker_QueueHasMessageNoExistingStorage_DoesNotCallDownloadText()
        {
            String msgId = String.Empty;
            String popReceipt = String.Empty;

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(fakeQueueMessage);

            Storage.Expect(storage => storage.Exists())
                .Return(false);

            Act();

            Storage.AssertWasNotCalled(storage => storage.DownloadText());
        }

        [Test]
        public void Worker_QueueHasMessageExistingStorage_UploadsSerializedDataToStorage()
        {
            String msgId;
            String popReceipt;
            const String newStorageContent = "{\"List\":[{\"FirstName\":\"Joel\",\"LastName\":\"Skrepnek\",\"Phrase\":\"see if you can read this JSON\",\"Date\":\"2012-08-07T08:04:04.8787972-07:00\"},{\"FirstName\":\"Joel\",\"LastName\":\"Skrepnek\",\"Phrase\":\"see if you can read this JSON\",\"Date\":\"2012-08-07T08:04:04.8787972-07:00\"}]}";

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(fakeQueueMessage)
                .OutRef(MSG_ID, POP_RECEIPT);

            Queue.Expect(queue => queue.DeleteMessage(Arg<string>.Is.Equal(MSG_ID), Arg<string>.Is.Equal(POP_RECEIPT)));

            Storage.Expect(storage => storage.Exists())
                .Return(true);

            Storage.Expect(storage => storage.DownloadText())
                .Return(fakeStorageText);

            Storage.Expect(storage => storage.UploadText(Arg<string>.Is.Equal(newStorageContent)));

            Act();

            Reader.VerifyAllExpectations();
            Queue.VerifyAllExpectations();
            Storage.VerifyAllExpectations();
        }

        [Test]
        public void Worker_ExistingStorageIsInvalid_NewMessageAddedToCleanStorage()
        {
            String msgId;
            String popReceipt;

            const string fakeInvalidStorageText = "{:[{\"LastName\":\"Skrepnek\",\"Phrase\":\"see if you can read this JSON\",\"Date\":\"2012-08-07T08:04:04.8787972-07:00\"}]}";

            Reader.Expect(reader => reader.KeyAvailable)
                .Return(true);

            Queue.Expect(queue => queue.GetMessage(out msgId, out popReceipt))
                .Return(fakeQueueMessage)
                .OutRef(MSG_ID, POP_RECEIPT);

            Storage.Expect(storage => storage.Exists())
                .Return(true);

            Storage.Expect(storage => storage.DownloadText())
                .Return(fakeInvalidStorageText);

            Storage.Expect(storage => storage.UploadText(Arg<string>.Is.Equal(fakeStorageText)));

            Act();

            Storage.VerifyAllExpectations();
        }

        //[Test]
        //public void Worker_ProcessedMessageThenKeyPressed_Exits()
        //{
        //    String msgId;
        //    String popReceipt;

        //    IKeyReader stubKeyReader = MockRepository.GenerateStub<IKeyReader>();
        //    IQueue stubQueue = MockRepository.GenerateStub<IQueue>();

        //    stubKeyReader.Expect(reader => reader.KeyAvailable).Return(false).Repeat.Once();
        //    stubKeyReader.Expect(reader => reader.KeyAvailable).Return(true);
            
        //    stubQueue.Expect(queue => queue.GetMessage(out msgId, out popReceipt)).Return(fakeQueueMessage).Repeat.Once();
        //    stubQueue.Expect(queue => queue.GetMessage(out msgId, out popReceipt)).Return(null).Repeat.Once();
        
        //    Storage.Expect(storage => storage.Exists())
        //        .Return(false).Repeat.Once();

        //    Act();

        //    Storage.VerifyAllExpectations();
        //}
    }
}
