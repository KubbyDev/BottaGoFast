import time
import Connection


def send_response(order):
    print(order)
    print("Response: " + order)
    Connection.send_command(order)
    global canStart
    canStart = True


canStart = False
Connection.set_order_event(send_response)

while(not canStart):
    Connection.update()

start = time.time()
frame = start

while (time.time() - start < 1):
    Connection.update()
    if(time.time() - frame > 0.001):
        print(time.time() - frame)
    frame = time.time()
