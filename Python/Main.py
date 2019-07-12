import time
import Connection


def send_response(order):
    print(order)
    response = order[:6] + str(int(order[6:])+1)
    print("Response: " + response)
    Connection.send_command(response)
    global canStart
    canStart = True


canStart = False
Connection.set_order_event(send_response)

while(not canStart):
    Connection.update()
    time.sleep(0.001)

start = time.time()

while (time.time() - start < 1):
    Connection.update()
    time.sleep(0.001)
