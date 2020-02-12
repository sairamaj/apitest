import time
import sys
import struct
import win32pipe, win32file, pywintypes

class PipeServer():
    def __init__(self):
        print('>>>>>>>>> in PipeServer __init___')
        self.connect()

    def send(self, message):
        try:
            win32pipe.ConnectNamedPipe(self.pipe, None)
            some_data = struct.pack('I', len(message)) + str.encode(message,'utf-8')
            print(type(some_data))
            print(some_data)
            win32file.WriteFile(self.pipe, some_data)
        except Exception as e:
            print(f'not connected {e}')
            print(f"type :{type(e)}")
            if e.winerror == 232:
                self.close()
                self.connect()

    def connect(self):
        self.pipe = win32pipe.CreateNamedPipe(
            r'\\.\pipe\Foo',
            win32pipe.PIPE_ACCESS_OUTBOUND,
            win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_NOWAIT,
            1, 65536, 65536,
            0,
            None)
    
    def close(self):
        win32file.CloseHandle(self.pipe)


