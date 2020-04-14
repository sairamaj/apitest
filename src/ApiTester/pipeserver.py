import time
import sys
import struct
import win32pipe, win32file, pywintypes
from ui import printWarning , printInfo

class PipeServer():
    def __init__(self, name):
        self.name = name
        self.connect()

    def send(self, message):
        try:
            self.saveToFile(message)
            win32pipe.ConnectNamedPipe(self.pipe, None)
            some_data = struct.pack('I', len(message)) + str.encode(message,'utf-8')
            win32file.WriteFile(self.pipe, some_data)
        except Exception as e:
            printWarning(str(e))
            if e.winerror == 232:
                self.close()
                self.connect()

    def connect(self):
        printInfo(f"pipserver.connecting: {self.name}")
        try:
            self.pipe = win32pipe.CreateNamedPipe(
            rf'\\.\pipe\{self.name}',
            win32pipe.PIPE_ACCESS_OUTBOUND,
            win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_NOWAIT,
            1, 65536, 65536,
            0,
            None)
        except Exception as e:
            printWarning(str(e))

    def close(self):
        printInfo('pipeserver.close')
        win32file.CloseHandle(self.pipe)

    def saveToFile(self, data):
        pass
        #with open(r'c:\temp\temp\send.json', 'w') as out_file:
        #    out_file.write(data)
