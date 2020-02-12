import time
import sys
import struct
import win32pipe, win32file, pywintypes


def pipeserver(message):
    print("pipe server")
    count = 0
    pipe = win32pipe.CreateNamedPipe(
        r'\\.\pipe\Foo',
        win32pipe.PIPE_ACCESS_OUTBOUND,
        win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_NOWAIT,
        1, 65536, 65536,
        0,
        None)
    try:
        print("waiting for client")
        win32pipe.ConnectNamedPipe(pipe, None)
        print("got client")

        some_data = struct.pack('I', len(message)) + str.encode(message,'utf-8')
        print(type(some_data))
        print(some_data)
        win32file.WriteFile(pipe, some_data)
    except Exception as e:
        print(f'not connected {e}')
    finally:
        win32file.CloseHandle(pipe)

